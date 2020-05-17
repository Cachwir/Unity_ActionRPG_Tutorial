using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bump : EnnemyAttack
{
    public float moveSpeed;
    public float moveAnimationSpeed = 1;
    public float playerMinDistanceToAttack;
    public float maxWalkToPlayerDuration;
    public float hitArea;
    public float repelDistance; // repels the target

    protected string moveId;
    protected float maxWalkToPlayerDurationCounter;

    protected float giveUpAfterMoveTime;
    
    public override void MoveOnPosition()
    {
        base.MoveOnPosition();
        isPositionToMoveToIsReached = false;
        maxWalkToPlayerDurationCounter = maxWalkToPlayerDuration;
        ennemyController.SetAnimationSpeed(moveAnimationSpeed);
    }

    protected override bool IsMoveOnPositionComplete()
    {
        float distanceBetweenEnnemyAndTarget = Vector3.Distance(target.transform.position, ennemyController.transform.position);
        return distanceBetweenEnnemyAndTarget <= playerMinDistanceToAttack;
    }

    public override void ExecuteWhileMovingOnPosition()
    {
        Vector3 move = Vector3.MoveTowards(ennemyController.transform.position, target.transform.position, Time.deltaTime * moveSpeed);
        move = Helper.MoveToVelocity(move, ennemyController.gameObject);
        moveId = ennemyController.MoveCompiler.AddOrEditMoveFactor(localIdInFile, move, moveId, true);

        maxWalkToPlayerDurationCounter -= Time.deltaTime;

        if (maxWalkToPlayerDurationCounter <= 0)
        {
            ExecuteOnMoveComplete();
            EndAttackWithoutHitting();
        }
    }

    public override void ExecuteOnMoveComplete()
    {
        ennemyController.MoveCompiler.RemoveMoveFactor(localIdInFile, moveId);
        ennemyController.ResetAnimationsSpeed();
    }

    public override bool CanDamageTarget()
    {
        return Vector3.Distance(target.transform.position, ennemyController.transform.position) <= hitArea && target.GetComponent<PlayerHealthManager>().CanTakeDamage();
    }

    public override void DamageTarget()
    {
        RepelTarget();
        base.DamageTarget();
    }

    protected void RepelTarget()
    {
        Movable movableTarget = target.GetComponent<Movable>();
        Vector3 repelForce = Helper.MultiplyVector3By((target.transform.position - ennemyController.transform.position).normalized, repelDistance * 10);
        float repelDuration = 0.1f;

        if (movableTarget != null)
        {
            movableTarget.RestrainMovements();
            MovementForce.AddMovementForce(movableTarget, repelForce, repelDuration, false, true, true, delegate()
            {
                movableTarget.UnrestrainMovements();
            });
        }
        else
        {
            // not supported, so not bumped
            Debug.Log("Unsupported bumpable target of localIdInFile " + Helper.GetObjectLocalIdInFile(this) + ". Make sure it has the Movable Component.");
        }
    }
}
