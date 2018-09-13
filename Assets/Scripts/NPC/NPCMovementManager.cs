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

    protected new void Update()
    {
        base.Update();
    }

    // moves only if there's a walk area and if the NPC isn't trying to leave that walk area
    protected override bool AdditionalMovingConditions()
    {
        if (IsMoving)
        {
            if (IsTryingToLeaveWalkArea())
            {
                StartWait();
            }
        }

        return base.AdditionalMovingConditions()
            && hasWalkArea
            && !IsTryingToLeaveWalkArea()
            ;
    }

    protected bool IsTryingToLeaveWalkArea()
    {
        return (MoveInput.x > 0 && transform.position.x > maxWalkPoint.x)
            || (MoveInput.y > 0 && transform.position.y > maxWalkPoint.y)
            || (MoveInput.x < 0 && transform.position.x < minWalkPoint.x)
            || (MoveInput.y < 0 && transform.position.y < minWalkPoint.y)
            ;
    }
}
