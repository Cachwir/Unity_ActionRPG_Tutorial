using UnityEngine;

/**
 * Waits
 */ 
public class Wait : EnnemyInFightMovementsPattern
{
    public override void ExecuteMovementsPattern()
    {
        ennemyCombatController.AddMovement(Vector2.zero, movementType);
    }
}
