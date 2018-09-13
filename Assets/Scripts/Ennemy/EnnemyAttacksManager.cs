using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyAttacksManager : MonoBehaviour
{
    public enum AttackManagementState
    {
        WAITING_FOR_ATTACK,
        ATTACKING
    }

    public float timeBetweenAttacks = 10.0f;
    public float timeBetweenAttacksDelta = 5.0f;

    protected List<EnnemyAttack> ennemyAttacks = new List<EnnemyAttack>();
    protected EnnemyAttack nextAttack;
    protected AttackManagementState attackManagementState;

    protected float timeBetweenAttacksCounter;

	// Use this for initialization
	void Start ()
    {
        ennemyAttacks.AddRange(GetComponents<EnnemyAttack>());
        ListenToAttacks();
        ResetAttackTiming();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        // HandleAttackTimingOnUpdate();
    }

    protected void HandleAttackTimingOnUpdate()
    {
        if (attackManagementState == AttackManagementState.WAITING_FOR_ATTACK)
        {
            timeBetweenAttacksCounter -= Time.fixedDeltaTime;

            if (timeBetweenAttacksCounter <= 0)
            {
                nextAttack.Attack();
            }
        }
        else if (attackManagementState == AttackManagementState.ATTACKING)
        {
            if (nextAttack.CurrentAttackStatus == EnnemyAttack.AttackStatus.FINISHED_ATTACKING)
            {

            }
        }
    }

    protected void ListenToAttacks()
    {
        foreach (EnnemyAttack ennemyAttack in ennemyAttacks)
        {
            ennemyAttack.AttacksManager = this;
        }
    }

    protected void ResetAttackTiming()
    {
        attackManagementState = AttackManagementState.WAITING_FOR_ATTACK;
        timeBetweenAttacksCounter = timeBetweenAttacks + Random.Range(-timeBetweenAttacksDelta, timeBetweenAttacksDelta);
        DecideOnAttack();
    }

    protected void DecideOnAttack()
    {
        // TODO : instead of deciding by random, we should use some kind of scale system=
        nextAttack = ennemyAttacks[Random.Range(0, ennemyAttacks.Count - 1)];
    }

    public void NotifyAttackEnded()
    {
        ResetAttackTiming();
    }
}
