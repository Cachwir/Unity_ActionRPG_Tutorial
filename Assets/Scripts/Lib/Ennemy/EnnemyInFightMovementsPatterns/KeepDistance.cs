using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Keeps a certain distance from the player
 */ 
public class KeepDistance : EnnemyInFightMovementsPattern
{
    public float distance;
    
    protected PlayerController playerController;

    // Use this for initialization
    protected new void Start ()
    {
        base.Start();
        executionType = ExecutionType.ON_UPDATE;
        playerController = FindObjectOfType<PlayerController>();
        ennemyCombatController = GetComponent<EnnemyCombatController>();
    }

    public override void ExecuteMovementsPattern()
    {
        Vector2 playerPosition = playerController.transform.position;
        Vector2 ennemyPosition = new Vector2(ennemyController.transform.position.x, ennemyController.transform.position.y);
        float actualDistance = (playerPosition - ennemyPosition).magnitude;
        Vector2 moveDirection;

        if (actualDistance > distance)
        {
            // let's get closer
            moveDirection = (playerPosition - ennemyPosition).normalized;
        }
        else if (actualDistance < distance)
        {
            // let's move away
            moveDirection = (ennemyPosition - playerPosition).normalized;
        }
        else
        {
            ennemyCombatController.StopMovement();
            return;
        }

        ennemyCombatController.AddMovement(moveDirection, movementType);
    }
}
