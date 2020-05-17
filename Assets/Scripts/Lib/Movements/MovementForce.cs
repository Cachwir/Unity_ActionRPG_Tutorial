using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementForce : MonoBehaviour
{
    public Vector3 force;
    public bool impactsAnimation;
    public bool hasDuration;
    public float duration;
    public bool isPaused = true;
    public MoveCompiler.Movable_OnMoveFactorEndCallback callback;
    public long localInIdFile;

    protected Movable thisMovable;
    protected string moveId;

    protected void Awake()
    {
        thisMovable = GetComponent<Movable>();
        localInIdFile = Helper.GetObjectLocalIdInFile(this);
    }

    public void StartMovementForce()
    {
        isPaused = false;
        moveId = thisMovable.MoveCompiler.AddMoveFactor(localInIdFile, force, impactsAnimation, false, hasDuration, duration, callback);
    }

    public void PauseMovementForce()
    {
        isPaused = true;
        duration = thisMovable.MoveCompiler.GetMoveFactor(localInIdFile, moveId).durationCounter;
        thisMovable.MoveCompiler.RemoveMoveFactor(localInIdFile, moveId, true);
    }

    public void StopMovementForce(bool ignoredOnEndCallback)
    {
        thisMovable.MoveCompiler.RemoveMoveFactor(localInIdFile, moveId);
    }

    public static MovementForce AddMovementForce(Movable target, Vector3 force, float duration = 0, bool impactsAnimation = false, bool autoStart = true, bool destroyOnEnd = true, MoveCompiler.Movable_OnMoveFactorEndCallback callback = null)
    {
        if (callback == null)
        {
            callback = delegate() { };
        }

        MovementForce movementForce = target.gameObject.AddComponent<MovementForce>();
        movementForce.force = force;
        movementForce.impactsAnimation = impactsAnimation;

        if (duration > 0)
        {
            movementForce.hasDuration = true;
            movementForce.duration = duration;
        }

        movementForce.callback = delegate ()
        {
            callback();

            if (destroyOnEnd)
            {
                Destroy(movementForce);
            }
        };

        if (autoStart)
        {
            movementForce.StartMovementForce();
        }

        return movementForce;
    }
}
