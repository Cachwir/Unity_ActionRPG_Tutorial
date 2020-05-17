using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementManager : AutoMoving
{
    protected bool hasWalkArea;
    protected Collider2D walkArea;

    protected Vector2 minWalkPoint;
    protected Vector2 maxWalkPoint;

    // Use this for initialization
    protected new void Start ()
    {
        base.Start();

        if (transform.parent != null)
        {
            hasWalkArea = true;
            walkArea = transform.parent.gameObject.GetComponent<Collider2D>();
            minWalkPoint = walkArea.bounds.min;
            maxWalkPoint = walkArea.bounds.max;
        }
        else
        {
            hasWalkArea = false;
        }
    }

    // moves only if there's a walk area and if the NPC isn't trying to leave that walk area
    public override bool ValidateMoveDirection(Vector3 velocity)
    {
        if (IsMoving)
        {
            if (IsTryingToLeaveWalkArea(velocity))
            {
                StartWait();
            }
        }

        return base.ValidateMoveDirection(velocity)
            && !hasWalkArea
            || (
                hasWalkArea
                && !IsTryingToLeaveWalkArea(velocity)
                )
            ;
    }

    protected bool IsTryingToLeaveWalkArea(Vector3 velocity)
    {
        return (velocity.x > 0 && transform.position.x > maxWalkPoint.x)
            || (velocity.y > 0 && transform.position.y > maxWalkPoint.y)
            || (velocity.x < 0 && transform.position.x < minWalkPoint.x)
            || (velocity.y < 0 && transform.position.y < minWalkPoint.y)
            ;
    }
}
