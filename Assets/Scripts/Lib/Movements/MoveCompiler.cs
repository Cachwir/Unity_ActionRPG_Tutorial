using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Move Compiler
 * This class handles and compiles all move forces affected to its movable object.
 */ 
public class MoveCompiler {

    public delegate void Movable_OnMoveFactorEndCallback();

    protected Movable movable;
    /*
     * long: localIdInFile
     * string: moveId
     * Vector3: move force factor
     */
    protected Dictionary<long, Dictionary<string, MoveFactorData>> moveFactorsContainer = new Dictionary<long, Dictionary<string, MoveFactorData>>();
    protected Vector3 compiledVelocity;
    protected long localIdInFile;
    protected Helper helper;
    protected string selfMoveId; // movable instance's moveId. Used to handle her own moves.

    public class MoveFactorData
    {
        public enum Type
        {
            DEFAULT,
            CINEMATIC
        }

        public Vector3 factor;
        public Type type = Type.DEFAULT;
        public bool isTimed;
        public float duration;
        public float durationCounter;
        public bool impactsAnimation;
        public Movable_OnMoveFactorEndCallback callback;

        public void Awake()
        {
            if (isTimed)
            {
                durationCounter = duration;
            }
        }

        public void LateUpdate()
        {
            if (isTimed)
            {
                durationCounter -= Time.deltaTime;
            }
        }

        public bool HasEnded()
        {
            return isTimed && durationCounter <= 0;
        }

        public void OnEnd()
        {
            if (callback != null)
            {
                callback();
            }
        }

        public bool IsCinematic()
        {
            return type == Type.CINEMATIC;
        }
    }

    public MoveCompiler(Movable targetMovable, Helper appHelper)
    {
        movable = targetMovable;
        localIdInFile = Helper.GetObjectLocalIdInFile(movable);
        helper = appHelper;
    }

    // Returns the compiled velocity
    public Vector3 Compile()
    {
        UpdateMoveFactorData();
        ValidateMoveFactors();
        CompileMoveFactors();

        return compiledVelocity;
    }

    protected void UpdateMoveFactorData()
    {
        foreach (long localIdInFile in new List<long>(moveFactorsContainer.Keys))
        {
            foreach (string moveId in new List<string>(moveFactorsContainer[localIdInFile].Keys))
            {
                moveFactorsContainer[localIdInFile][moveId].LateUpdate();
            }
        }
    }

    protected void ValidateMoveFactors()
    {
        // manipulating a dictionary while iterating over it is a big no no, se we create a buffer
        foreach (long localIdInFile in new List<long>(moveFactorsContainer.Keys))
        {
            if (!IsMoveFactorsSourceStillUp(localIdInFile))
            {
                RemoveMoveFactors(localIdInFile);
            }
            else
            {
                foreach (string moveId in new List<string>(moveFactorsContainer[localIdInFile].Keys))
                {
                    if (selfMoveId != moveId || !movable.AreMovementsRestrained)
                    {
                        if (!IsMoveFactorSourceStillUp(moveFactorsContainer[localIdInFile][moveId]))
                        {
                            RemoveMoveFactor(localIdInFile, moveId);
                        }
                    }
                }
            }
        }
    }

    protected void CompileMoveFactors()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 animationDirection = Vector3.zero;

        foreach (KeyValuePair<long, Dictionary<string, MoveFactorData>> moveFactors in moveFactorsContainer)
        {
            foreach (KeyValuePair<string, MoveFactorData> moveFactor in moveFactors.Value)
            {
                if (IsMoveFactorCompilable(moveFactor.Value))
                {
                    velocity += moveFactor.Value.factor;

                    if (moveFactor.Value.impactsAnimation)
                    {
                        animationDirection += moveFactor.Value.factor;
                    }
                }
            }
        }

        if (Helper.IsV3Equal(animationDirection, Vector3.zero))
        {
            movable.IsMoving = false;
        }
        else
        {
            movable.IsMoving = true;
            animationDirection = animationDirection.normalized;
            movable.MoveInput = animationDirection;
            movable.LastMove = animationDirection;
        }

        if (movable.is2D)
        {
            velocity = new Vector3(velocity.x, velocity.y, 0);
        }
        
        compiledVelocity = velocity;

        if (!movable.ValidateMoveDirection(velocity))
        {
            compiledVelocity = Vector3.zero;
        }
    }

    protected bool IsMoveFactorCompilable(MoveFactorData moveFactorData)
    {
        return 
            !movable.IsImmobilized
            && (
                !movable.IsInCinematicMode
                || moveFactorData.IsCinematic()
            );
    }

    protected bool IsMoveFactorsSourceStillUp(long localInFileId)
    {
        return helper.FindObjectByLocalIdInFile(localInFileId) != null;
    }

    protected bool IsMoveFactorSourceStillUp(MoveFactorData moveFactorData)
    {
        return !moveFactorData.HasEnded();
    }

    /**
     * Adds a move factor.
     * Important : store the returned moveId to the caller's side. You'll need it to remove the move factor if you don't remove'em all at the same time.
     */
    public string AddMoveFactor(long localIdInFile, Vector3 move, bool impactsAnimation = false, bool isCinematic = false, bool hasDuration = false, float duration = 0, Movable_OnMoveFactorEndCallback callback = null)
    {
        if (!moveFactorsContainer.ContainsKey(localIdInFile))
        {
            moveFactorsContainer.Add(localIdInFile, new Dictionary<string, MoveFactorData>());
        }

        string moveId = Helper.GenerateUniqueID();

        MoveFactorData moveFactorData = new MoveFactorData
        {
            factor = move,
            type = isCinematic ? MoveFactorData.Type.CINEMATIC : MoveFactorData.Type.DEFAULT,
            isTimed = hasDuration,
            duration = duration,
            impactsAnimation = impactsAnimation,
            callback = callback
        };

        moveFactorData.Awake();

        moveFactorsContainer[localIdInFile].Add(moveId, moveFactorData);

        return moveId;
    }
    
    public string AddOrEditMoveFactor(long localIdInFile, Vector3 move, string moveId = null, bool impactsAnimation = false, bool isCinematic = false, bool hasDuration = false, float duration = 0, Movable_OnMoveFactorEndCallback callback = null)
    {
        if (GetMoveFactor(localIdInFile, moveId) != null)
        {
            RemoveMoveFactor(localIdInFile, moveId, true);
        }

        return AddMoveFactor(localIdInFile, move, impactsAnimation, isCinematic, hasDuration, duration, callback);
    }

    // /!\ Can only handle one source
    public void AddOrEditSelfMoveFactor(Vector3 move, bool hasDuration = false, float duration = 0, Movable_OnMoveFactorEndCallback callback = null)
    {
        selfMoveId = AddOrEditMoveFactor(localIdInFile, move, selfMoveId, true, false, hasDuration, duration, callback);
    }

    public MoveFactorData GetMoveFactor(long localIdInFile, string moveId)
    {
        if (moveFactorsContainer.ContainsKey(localIdInFile) && moveFactorsContainer[localIdInFile].ContainsKey(moveId))
        {
            return moveFactorsContainer[localIdInFile][moveId];
        }
        else
        {
            return null;
        }
    }

    public MoveFactorData GetSelfMoveFactor()
    {
        return GetMoveFactor(localIdInFile, selfMoveId);
    }

    public void RemoveMoveFactors(long localIdInFile)
    {
        if (moveFactorsContainer.ContainsKey(localIdInFile))
        {
            foreach (KeyValuePair<string, MoveFactorData> moveFactor in moveFactorsContainer[localIdInFile])
            {
                moveFactor.Value.OnEnd();
            }

            moveFactorsContainer.Remove(localIdInFile);
        }
    }

    public void RemoveMoveFactor(long localIdInFile, string moveId, bool ignoredOnEndCallbacks = false)
    {
        if (moveFactorsContainer.ContainsKey(localIdInFile) && moveFactorsContainer[localIdInFile].ContainsKey(moveId))
        {
            if (!ignoredOnEndCallbacks)
            {
                moveFactorsContainer[localIdInFile][moveId].OnEnd();
            }

            moveFactorsContainer[localIdInFile].Remove(moveId);
        }
    }

    public void RemoveSelfMoveFactor(bool ignoredOnEndCallbacks = false)
    {
        RemoveMoveFactor(localIdInFile, selfMoveId, ignoredOnEndCallbacks);
    }

    public void ResetMoveFactors(long localIdInFile, bool ignoredOnEndCallbacks = false)
    {
        if (!ignoredOnEndCallbacks)
        {
            foreach (KeyValuePair<long, Dictionary<string, MoveFactorData>> moveFactors in moveFactorsContainer)
            {
                foreach (KeyValuePair<string, MoveFactorData> moveFactor in moveFactors.Value)
                {
                    moveFactor.Value.OnEnd();
                }
            }
        }

        moveFactorsContainer.Clear();
    }
}
