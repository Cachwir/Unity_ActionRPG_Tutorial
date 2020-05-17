using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnnemyInFightMovementsPattern : MonoBehaviour, IVoter
{
    public delegate void EnnemyInFightMovementsPattern_Callback();

    public enum ExecutionType
    {
        ONCE, // Use this is the movement is rectiline (lighter)
        ON_UPDATE, // Use this if the movement is not rectiline
    }

    public float movementDuration;
    public float movementDurationDelta = 0.25f;
    public float weight; // higher number = higher priority

    protected ExecutionType executionType;
    protected EnnemyController ennemyController;
    protected EnnemyCombatController ennemyCombatController;
    protected float movementDurationCounter;
    protected EnnemyCombatController.MovementType movementType;

    public bool IsExecuting { get; set; }
    public EnnemyInFightMovementsPattern_Callback OnExecutionEnd { get; set; }

    // Use this for initialization
    protected virtual void Start ()
    {
        ennemyController = GetComponent<EnnemyController>();
        ennemyCombatController = GetComponent<EnnemyCombatController>();
        movementType = EnnemyCombatController.MovementType.NORMAL;
    }
	
	// Update is called once per frame
	protected virtual void Update ()
    {
		if (executionType == ExecutionType.ON_UPDATE && IsExecuting)
        {
            ExecuteMovementsPattern();
        }
        if (IsExecuting)
        {
            movementDurationCounter -= Time.deltaTime;

            if (movementDurationCounter <= 0)
            {
                IsExecuting = false;
                OnExecutionEnd();
            }
        }
	}

    public void Execute()
    {
        IsExecuting = true;
        movementDurationCounter = Random.Range(movementDuration * (1 - movementDurationDelta), movementDuration * (1 + movementDurationDelta));

        if (executionType == ExecutionType.ONCE)
        {
            ExecuteMovementsPattern();
        }
    }

    abstract public void ExecuteMovementsPattern();

    public virtual void StopMovementsPattern(bool ignoreCallback = false)
    {
        IsExecuting = false;
        OnExecutionEnd();
        ennemyCombatController.RemoveMoveFactor(ignoreCallback);
    }

    public float GetWeight()
    {
        return weight;
    }
}
