using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnnemyAttack : HurtPlayer
{
    public enum AttackStatus {
        UNSTARTED,              // ready to attack
        MOVING_ON_POSITION,     // moves to the specific position
        PREPARING,              // prepares the attack
        FINISHED_PREPARING,     // has finished preparing the attack
        ATTACKING,              // starts the attack
        HIT,                    // hits the target
        FINISHED_ATTACKING,     // has finished its attack
        COOLDOWN,               // is on cooldown
    };
    
    public float prepareTime;
    public float attackTime;
    public float cooldownTime;
    public float hitTime;

    public bool doesPrepareAnimationDependOnPrepareTime;
    public bool doesAttackAnimationDependOnAttackTime;
    public bool doesHitAnimationDependOnHitTime;

    public float prepareTimeDelta;
    public float cooldownTimeDelta;

    public string prepareAnimationTrigger;
    public string executeAnimationTrigger;
    public string hitAnimationTrigger;

    public bool stopAttackOnHit;

    protected EnnemyController ennemyController;
    protected Animator animator;
    protected GameObject target;
    public AttackStatus CurrentAttackStatus { get; set; }
    public EnnemyAttacksManager AttacksManager { get; set; }

    protected float prepareTimeCounter;
    protected float attackTimeCounter;
    protected float hitTimeCounter;
    protected float cooldownTimeCounter;

    protected float animatorBaseSpeed;
    protected bool isInContactWithTarget;

    protected Vector3 attackStartPosition;
    protected bool isPositionToMoveToIsReached;

    protected long localIdInFile;

    // Use this for initialization
    protected new void Start ()
    {
        base.Start();

        localIdInFile = Helper.GetObjectLocalIdInFile(this);
        ennemyController = GetComponent<EnnemyController>();
        animator = ennemyController.GetComponent<Animator>();
        target = FindObjectOfType<PlayerController>().gameObject;

        animatorBaseSpeed = animator.speed;
    }
	
	// Update is called once per frame
	void Update ()
    {
        HandleAttackStatusOnUpdate();
    }

    protected void LateUpdate()
    {
        
    }

    protected void HandleAttackStatusOnUpdate()
    {
        switch (CurrentAttackStatus)
        {
            case AttackStatus.MOVING_ON_POSITION:

                if (!IsMoveOnPositionComplete())
                {
                    ExecuteWhileMovingOnPosition(); // Move the ennemyController to the position
                }
                else
                {
                    ExecuteOnMoveComplete();
                    PrepareAttack();
                }
                break;

            case AttackStatus.PREPARING:
                prepareTimeCounter -= Time.deltaTime;

                ExecuteWhilePreparing();

                if (prepareTimeCounter <= 0)
                {
                    CurrentAttackStatus = AttackStatus.FINISHED_PREPARING;
                    if (doesAttackAnimationDependOnAttackTime)
                    {
                        ennemyController.SetAnimationSpeed(1 / attackTime);
                    }
                    else
                    {
                        ennemyController.ResetAnimationsSpeed();
                    }
                    ExecuteAttack();
                }
                break;

            case AttackStatus.ATTACKING:
                attackTimeCounter -= Time.deltaTime;

                ExecuteWhileAttacking();

                if (CanDamageTarget())
                {
                    DamageTarget();
                }

                if (attackTimeCounter <= 0)
                {
                    CurrentAttackStatus = AttackStatus.FINISHED_ATTACKING;
                    EndAttackWithoutHitting();
                }
                break;

            case AttackStatus.HIT:
                hitTimeCounter -= Time.deltaTime;

                ExecuteWhileHitting();

                if (hitTimeCounter <= 0)
                {
                    if (hitAnimationTrigger != "")
                    {
                        animator.SetBool(hitAnimationTrigger, false);
                    }

                    EndAttack();
                }
                break;

            case AttackStatus.COOLDOWN:
                cooldownTimeCounter -= Time.deltaTime;

                if (cooldownTimeCounter <= 0)
                {
                    CurrentAttackStatus = AttackStatus.UNSTARTED;
                }
                break;
        }
    }

    protected override void OnCollisionStay2D(Collision2D other)
    {
        // nothing, we handle the damage differently
    }

    protected void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            isInContactWithTarget = true;
        }
    }

    protected void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            isInContactWithTarget = false;
        }
    }

    protected virtual bool IsMoveOnPositionComplete()
    {
        return isPositionToMoveToIsReached;
    }

    protected void SetPrepareTimeCounter()
    {
        prepareTimeCounter = prepareTime + Random.Range(-prepareTimeDelta, prepareTimeDelta);
    }

    protected void SetAttackTimeCounter()
    {
        attackTimeCounter = attackTime;
    }

    protected void SetHitTimeCounter()
    {
        hitTimeCounter = hitTime;
    }

    protected void SetCooldownTimeCounter()
    {
        cooldownTimeCounter = cooldownTime + Random.Range(-cooldownTimeDelta, cooldownTimeDelta);
    }

    public virtual bool CanAttack()
    {
        return true;
    }

    public void Attack()
    {
        // The attack can only be started if it's unstarted
        if (CurrentAttackStatus == AttackStatus.UNSTARTED)
        {
            isPositionToMoveToIsReached = false;
            SetAttackStartPosition();
            ennemyController.InterruptMoving();
            MoveOnPosition();
        }
    }

    public virtual void SetAttackStartPosition()
    {
        attackStartPosition = ennemyController.transform.position;
    }

    public virtual void MoveOnPosition()
    {
        isPositionToMoveToIsReached = true;
        CurrentAttackStatus = AttackStatus.MOVING_ON_POSITION;
    }

    public virtual void PrepareAttack()
    {
        GetComponent<EnnemyController>().InterruptMoving();

        SetPrepareTimeCounter();
        CurrentAttackStatus = AttackStatus.PREPARING;

        if (doesPrepareAnimationDependOnPrepareTime)
        {
            ennemyController.SetAnimationSpeed(1 / prepareTime);
        }
        else
        {
            ennemyController.ResetAnimationsSpeed();
        }

        if (hitAnimationTrigger != "")
        {
            animator.SetBool(hitAnimationTrigger, false);
        }
        
        animator.SetBool(prepareAnimationTrigger, true);
    }

    public virtual void ExecuteAttack()
    {
        SetAttackTimeCounter();
        CurrentAttackStatus = AttackStatus.ATTACKING;

        animator.SetBool(executeAnimationTrigger, true);
        animator.SetBool(prepareAnimationTrigger, false);
    }

    public virtual bool CanDamageTarget()
    {
        return isInContactWithTarget && target.GetComponent<PlayerHealthManager>().CanTakeDamage();
    }

    public virtual void DamageTarget()
    {
        TryHurt(target, GetDamageAmount());

        if (stopAttackOnHit)
        {
            TriggerHit(); // triggers the hit status only if the attack process should be stopped
        }
    }

    public virtual void ExecuteWhileMovingOnPosition() { }

    public virtual void ExecuteOnMoveComplete() { }

    public virtual void ExecuteWhilePreparing() {}

    public virtual void ExecuteWhileAttacking() {}

    public virtual void ExecuteWhileHitting() {}

    public virtual void TriggerHit()
    {
        if (doesHitAnimationDependOnHitTime)
        {
            ennemyController.SetAnimationSpeed(1 / hitTime);
        }
        else
        {
            ennemyController.ResetAnimationsSpeed();
        }

        SetHitTimeCounter();
        CurrentAttackStatus = AttackStatus.HIT;

        if (hitAnimationTrigger != "")
        {
            animator.SetBool(hitAnimationTrigger, true);
            animator.SetBool(executeAnimationTrigger, false);
        }
    }

    public virtual void InterruptAttack(bool activateCoolDown = true)
    {
        EndAttack(activateCoolDown);
    }

    public virtual void EndAttackWithoutHitting(bool activateCoolDown = true)
    {
        animator.SetBool(executeAnimationTrigger, false);
        EndAttack(activateCoolDown);
    }

    public virtual void EndAttack(bool activateCoolDown = true)
    {
        ennemyController.ResetAnimationsSpeed();
        animator.SetBool(executeAnimationTrigger, false);

        if (activateCoolDown)
        {
            SetCooldownTimeCounter();
            CurrentAttackStatus = AttackStatus.COOLDOWN;
        }
        else
        {
            CurrentAttackStatus = AttackStatus.UNSTARTED;
        }
        
        ennemyController.StartMoving();
        AttacksManager.NotifyAttackEnded();
    }
}
