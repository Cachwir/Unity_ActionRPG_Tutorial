using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : EnnemyAttack
{
    public float maxChargeDistance;
    public float bouncingDistance; // after hitting target

    protected Vector3 chargeTarget;
    protected Vector3 landTarget;
    protected float chargeSpeed;
    protected float landingSpeed;
    protected Vector3 beforeChargePosition;

    public override void PrepareAttack()
    {
        GetComponent<EnnemyController>().InterruptMoving();

        SetPrepareTimeCounter();
        CurrentAttackStatus = AttackStatus.PREPARING;

        animator.SetBool(hitAnimationTrigger, false);
        animator.SetBool(prepareAnimationTrigger, true);
    }

    public override void ExecuteAttack()
    {
        SetAttackTimeCounter();
        CurrentAttackStatus = AttackStatus.ATTACKING;
        
        SetChargeTarget();
        SetChargeSpeed();

        animator.SetBool(executeAnimationTrigger, true);
        animator.SetBool(prepareAnimationTrigger, false);
    }

    public void SetChargeTarget()
    {
        beforeChargePosition = Helper.CopyVector3(ennemyController.transform.position);
        Vector3 monsterToTargetVector = target.transform.position - beforeChargePosition; // get the Vector representing the distance between the ennemy and the target

        float distanceBetweenEnnemyAndTarget = Vector3.Distance(target.transform.position, beforeChargePosition);
        float distanceRatio = maxChargeDistance / distanceBetweenEnnemyAndTarget;

        // gets the Vector representing the distance between the monster and the charge end point
        Vector3 monsterToChargeTargetVector = new Vector3(monsterToTargetVector.x * distanceRatio, monsterToTargetVector.y * distanceRatio, 0);

        // apply this vector to the ennemy's position to obtain the position of the end of the charge
        chargeTarget = ennemyController.transform.position + monsterToChargeTargetVector;
    }

    public void SetLandTarget()
    {
        Vector3 monsterToBeforeChargePositionVector = beforeChargePosition - ennemyController.transform.position;

        float distanceBetweenEnnemyAndBeforeChargePosition = Vector3.Distance(beforeChargePosition, ennemyController.transform.position);
        float distanceRatio = bouncingDistance / distanceBetweenEnnemyAndBeforeChargePosition;

        Vector3 monsterToBouncePositionVector = new Vector3(monsterToBeforeChargePositionVector.x * distanceRatio, monsterToBeforeChargePositionVector.y * distanceRatio, 0);

        landTarget = ennemyController.transform.position + monsterToBouncePositionVector;
    }

    public void SetChargeSpeed()
    {
        chargeSpeed = Vector3.Distance(chargeTarget, ennemyController.transform.position) / attackTime;
    }

    public void SetLandSpeed()
    {
        landingSpeed = Vector3.Distance(landTarget, ennemyController.transform.position) / hitTime;
    }

    public override void DamageTarget()
    {
        int damageAmount = GetDamageAmount();

        TryHurt(target, damageAmount);

        SetLandTarget();
        SetLandSpeed();

        TriggerHit();
    }

    void OnAnimatorMove()
    {
        if (animator.GetBool(executeAnimationTrigger))
        {
            ennemyController.transform.position = Vector3.MoveTowards(ennemyController.transform.position, chargeTarget, Time.deltaTime * chargeSpeed);
        }
        else if (animator.GetBool(hitAnimationTrigger))
        {
            ennemyController.transform.position = Vector3.MoveTowards(ennemyController.transform.position, landTarget, Time.deltaTime * landingSpeed);
        }
    }
}