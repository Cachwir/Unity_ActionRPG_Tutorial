using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Handles the ennemy's movements in fight 
 */ 
public class EnnemyCombatController : MonoBehaviour
{
    public enum MovementType
    {
        NORMAL,
        DASH,
        BLINK // TODO : implement
    }

    public float moveSpeedInFight;
    public float dashMoveSpeed = 20;
    protected float blinkMoveSpeed = 100;

    protected EnnemyController ennemyController;
    protected List<EnnemyInFightMovementsPattern> movementPatterns;
    protected EnnemyInFightMovementsPattern currentMovementPattern;
    protected string moveId;
    protected long localIdInFile;

    public bool CanMove { get; set; }

    private void Awake()
    {
        ennemyController = GetComponent<EnnemyController>();
        movementPatterns = new List<EnnemyInFightMovementsPattern>(GetComponents<EnnemyInFightMovementsPattern>());
        localIdInFile = Helper.GetObjectLocalIdInFile(this);
        CanMove = true;
    }

    // Use this for initialization
    void Start ()
    {
        

    }
	
	// Update is called once per frame
	void Update ()
    {
		if (ennemyController.IsInCombat && CanMove)
        {
            if (currentMovementPattern == null)
            {
                currentMovementPattern = DecideMovement();
                currentMovementPattern.OnExecutionEnd = delegate ()
                {
                    currentMovementPattern = null;
                };
                currentMovementPattern.Execute();
            }
        }
	}
    
    public EnnemyInFightMovementsPattern DecideMovement()
    {
        return (EnnemyInFightMovementsPattern) VoteManager.Vote(new List<IVoter>(movementPatterns.ToArray()));
    }

    public void AddMovement(Vector2 moveDirection, MovementType movementType, float duration = 0, MoveCompiler.Movable_OnMoveFactorEndCallback callback = null)
    {
        bool impactsAnimation = movementType == MovementType.NORMAL;
        float moveSpeed;

        switch (movementType)
        {
            case MovementType.NORMAL:
                moveSpeed = moveSpeedInFight;
                break;

            case MovementType.DASH:
                moveSpeed = dashMoveSpeed;
                break;

            case MovementType.BLINK:
                // TODO do a real blink
                moveSpeed = blinkMoveSpeed;
                break;

            default:
                throw new System.Exception("Unhandled movementType " + movementType);
        }

        moveId = ennemyController.MoveCompiler.AddOrEditMoveFactor(localIdInFile, moveDirection * moveSpeed, moveId, impactsAnimation, false, duration > 0, duration, callback);
    }

    public void StopMovement(bool ignoreCallback = false)
    {
        currentMovementPattern.StopMovementsPattern(ignoreCallback);
    }
    
    public void RemoveMoveFactor(bool ignoreCallback = false)
    {
        currentMovementPattern = null;

        if (moveId != null)
        {
            ennemyController.MoveCompiler.RemoveMoveFactor(localIdInFile, moveId, ignoreCallback);
        }
    }

    public void ResumeMovements()
    {
        CanMove = true;
    }
}
