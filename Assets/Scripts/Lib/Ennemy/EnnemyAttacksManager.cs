using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyAttacksManager : MonoBehaviour
{
    public enum AttackManagementState
    {
        WAITING_FOR_DECISION,
        WAITING_FOR_ATTACK,
        ATTACKING
    }

    public float timeBetweenAttacks = 10.0f;
    public float timeBetweenAttacksDelta = 5.0f;

    protected EnnemyCombatController ennemyCombatController;
    protected List<EnnemyAttack> ennemyAttacks = new List<EnnemyAttack>();
    protected EnnemyAttack nextAttack;
    protected AttackManagementState attackManagementState;

    protected float timeBetweenAttacksCounter;
    protected bool canUseAttacks;

	// Use this for initialization
	void Start ()
    {
        ennemyCombatController = GetComponent<EnnemyCombatController>();
        attackManagementState = AttackManagementState.WAITING_FOR_DECISION;
        ennemyAttacks.AddRange(GetComponents<EnnemyAttack>());
        ListenToAttacks();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        HandleAttackTimingOnUpdate();
    }

    protected void HandleAttackTimingOnUpdate()
    {
        if (attackManagementState == AttackManagementState.WAITING_FOR_ATTACK)
        {
            timeBetweenAttacksCounter -= Time.deltaTime;

            if (timeBetweenAttacksCounter <= 0)
            {
                ennemyCombatController.StopMovement();
                nextAttack.Attack();
            }
        }
        else if (attackManagementState == AttackManagementState.WAITING_FOR_DECISION)
        {
            ResetAttackTiming();
        }
    }

    public void StartUsingAttacks()
    {
        canUseAttacks = true;
        ResetAttackTiming();
    }

    public void StopUsingAttacks()
    {
        canUseAttacks = false;
        attackManagementState = AttackManagementState.WAITING_FOR_DECISION;
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
        if (canUseAttacks && ennemyAttacks.Count > 0)
        {
            bool couldDecide = DecideOnAttack();

            if (couldDecide)
            {
                attackManagementState = AttackManagementState.WAITING_FOR_ATTACK;
                timeBetweenAttacksCounter = timeBetweenAttacks + Random.Range(-timeBetweenAttacksDelta, timeBetweenAttacksDelta);
            }
        }
    }

    // Returns true if an attack could be decided
    protected bool DecideOnAttack()
    {
        List<EnnemyAttack> availableEnnemyAttacks = GetAvailableEnnemyAttacks();

        if (availableEnnemyAttacks.Count > 0)
        {
            // TODO : instead of deciding by random, we should use some kind of scale system
            nextAttack = ennemyAttacks[Random.Range(0, ennemyAttacks.Count - 1)];
            return true;
        }

        return false;
    }

    public List<EnnemyAttack> GetAvailableEnnemyAttacks()
    {
        List<EnnemyAttack> availableEnnemyAttacks = new List<EnnemyAttack>();

        foreach (EnnemyAttack ennemyAttack in ennemyAttacks)
        {
            if (ennemyAttack.CanAttack())
            {
                availableEnnemyAttacks.Add(ennemyAttack);
            }
        }

        return availableEnnemyAttacks;
    }

    public void NotifyAttackEnded()
    {
        ennemyCombatController.ResumeMovements();
        ResetAttackTiming();
    }
}
