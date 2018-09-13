using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnnemyAttack : HurtPlayer
{
    public enum AttackStatus {
        UNSTARTED,
        PREPARING,
        FINISHED_PREPARING,
        ATTACKING,
        FINISHED_ATTACKING,
        COOLDOWN,
    };
    
    public float prepareTime;
    public float attackTime;
    public float cooldownTime;

    public bool doesPrepareAnimationDependOnPrepareTime;
    public bool doesAttackAnimationDependOnAttackTime;

    public float prepareTimeDelta;
    public float cooldownTimeDelta;

    public string prepareAnimationTrigger;
    public string executeAnimationTrigger;

    protected EnnemyController ennemyController;
    protected Animator animator;
    protected GameObject target;
    public AttackStatus CurrentAttackStatus { get; set; }
    public EnnemyAttacksManager AttacksManager { get; set; }

    protected float prepareTimeCounter;
    protected float attackTimeCounter;
    protected float cooldownTimeCounter;

    protected float animatorBaseSpeed;
    protected bool isInContactWithTarget;

    // Use this for initialization
    void Start ()
    {
        ennemyController = GetComponent<EnnemyController>();
        animator = ennemyController.GetComponent<Animator>();
        target = FindObjectOfType<PlayerController>().gameObject;

        animatorBaseSpeed = animator.speed;
    }
	
	// Update is called once per frame
	void Update () {

	}

    private void FixedUpdate()
    {
        switch (CurrentAttackStatus)
        {
            case AttackStatus.PREPARING:
                prepareTimeCounter -= Time.fixedDeltaTime;

                ExecuteWhilePreparing();

                if (prepareTimeCounter <= 0)
                {
                    CurrentAttackStatus = AttackStatus.FINISHED_PREPARING;
                    if (doesAttackAnimationDependOnAttackTime)
                    {
                        animator.speed = attackTime;
                    }
                    else
                    {
                        animator.speed = animatorBaseSpeed;
                    }
                    ExecuteAttack();
                }
                break;

            case AttackStatus.ATTACKING:
                attackTimeCounter -= Time.fixedDeltaTime;

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

            case AttackStatus.COOLDOWN:
                cooldownTimeCounter -= Time.fixedDeltaTime;

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

    public void Attack()
    {
        // The attack can only be started if it's unstarted
        if (CurrentAttackStatus == AttackStatus.UNSTARTED)
        {
            if (doesPrepareAnimationDependOnPrepareTime)
            {
                animator.speed = prepareTime;
            }
            else
            {
                animator.speed = animatorBaseSpeed;
            }
            ennemyController.InterruptMoving();
            PrepareAttack();
        }
    }

    public abstract void PrepareAttack();

    public abstract void ExecuteAttack();

    public abstract bool CanDamageTarget();

    public abstract void DamageTarget();

    public virtual void ExecuteWhilePreparing() {}

    public virtual void ExecuteWhileAttacking() {}

    public virtual void InterruptAttack(bool activateCoolDown = true)
    {
        EndAttack(activateCoolDown);
    }

    public virtual void EndAttackWithoutHitting(bool activateCoolDown = true)
    {
        EndAttack(activateCoolDown);
    }

    public virtual void EndAttack(bool activateCoolDown = true)
    {
        animator.speed = animatorBaseSpeed;

        if (activateCoolDown)
        {
            cooldownTimeCounter = cooldownTime;
            CurrentAttackStatus = AttackStatus.COOLDOWN;
        }
        else
        {
            CurrentAttackStatus = AttackStatus.UNSTARTED;
        }

        ennemyController.StartMove();
        AttacksManager.NotifyAttackEnded();
    }
}
