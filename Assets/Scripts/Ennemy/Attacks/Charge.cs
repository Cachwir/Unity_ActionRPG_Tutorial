using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : EnnemyAttack
{
    public string hitAnimationTrigger;
    public float maxChargeDistance;

    protected Vector3 chargeTarget;
    

    public override void PrepareAttack()
    {
        GetComponent<EnnemyController>().InterruptMoving();

        prepareTimeCounter = prepareTime;
        CurrentAttackStatus = AttackStatus.PREPARING;

        animator.SetBool(hitAnimationTrigger, false);
        animator.SetBool(prepareAnimationTrigger, true);
    }

    public override void ExecuteAttack()
    {
        attackTimeCounter = attackTime;
        CurrentAttackStatus = AttackStatus.ATTACKING;

        SetChargeTarget();

        animator.SetBool(executeAnimationTrigger, true);
        animator.SetBool(prepareAnimationTrigger, false);
    }

    public void SetChargeTarget()
    {
        Vector3 monsterToTargetVector = target.transform.position - ennemyController.transform.position; // get the Vector representing the distance between the ennemy and the target

        float distanceBetweenEnnemyAndTarget = Vector3.Distance(target.transform.position, ennemyController.transform.position);
        float distanceRatio = maxChargeDistance / distanceBetweenEnnemyAndTarget;

        // gets the Vector representing the distance between the monster and the charge end point
        Vector3 monsterToChargeTargetVector = new Vector3(monsterToTargetVector.x * distanceRatio, monsterToTargetVector.y * distanceRatio, ennemyController.transform.position.z);

        // apply this vector to the ennemy's position to obtain the position of the end of the charge
        chargeTarget = ennemyController.transform.position + monsterToChargeTargetVector;
    }

    public override void ExecuteWhileAttacking()
    {
        ennemyController.transform.position = Vector3.Lerp(ennemyController.transform.position, chargeTarget, attackTime - attackTimeCounter);
    }

    public override bool CanDamageTarget()
    {
        return isInContactWithTarget;
    }

    public override void DamageTarget()
    {
        animator.speed = animatorBaseSpeed;

        int damageAmount = GetDamageAmount();

        target.GetComponent<PlayerHealthManager>().HurtPlayer(damageAmount);
        PlayHurtEffect(target, damageAmount);

        CurrentAttackStatus = AttackStatus.FINISHED_ATTACKING;

        animator.SetBool(hitAnimationTrigger, true);
        animator.SetBool(executeAnimationTrigger, false);
        EndAttack();
    }
}