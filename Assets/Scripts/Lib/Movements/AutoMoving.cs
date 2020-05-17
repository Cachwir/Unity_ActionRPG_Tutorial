using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoving : SelfMovable
{
    public enum AutomoveDirectionType
    {
        ANY_DIRECTION,
        FOUR_DIRECTIONS
    }

    public float averageMoveTime;
    public float averageMoveTimeDelta = 0.25f;
    public float betweenMovesAverageTime;
    public float betweenMovesAverageTimeDelta = 0.25f;
    public AutomoveDirectionType moveDirectionType = AutomoveDirectionType.ANY_DIRECTION;

    public float TimeBetweenMovesCounter { get; set; }
    public float MoveTimeCounter { get; set; }
    public bool CanMove { get; set; }

    // Use this for initialization
    protected new void Start()
    {
        base.Start();
        
        MoveTimeCounter = Random.Range(averageMoveTime * (1 - averageMoveTimeDelta), averageMoveTime * (1 + averageMoveTimeDelta));
        TimeBetweenMovesCounter = Random.Range(betweenMovesAverageTime * (1 - betweenMovesAverageTimeDelta), betweenMovesAverageTime * (1 + betweenMovesAverageTimeDelta));

        StartMovingRandomly();
    }

    // Update is called once per frame
    protected new void Update ()
    {
        base.Update();
	}

    protected new void FixedUpdate()
    {
        HandleMovesOnUpdate();
        base.FixedUpdate();
    }

    public void HandleMovesOnUpdate()
    {
        if (CanMove)
        {
            if (!IsInCinematicMode && isMovingSelf)
            {
                MoveTimeCounter -= Time.deltaTime;

                if (!AreMovementsRestrained)
                {
                    MoveCompiler.AddOrEditSelfMoveFactor(selfMove);
                }

                if (MoveTimeCounter <= 0)
                {
                    StartWait();
                }
            }
            else
            {
                TimeBetweenMovesCounter -= Time.deltaTime;

                if (TimeBetweenMovesCounter <= 0)
                {
                    StartMove();
                }
            }
        }
    }

    public override bool ValidateMoveDirection(Vector3 velocity)
    {
        return base.ValidateMoveDirection(velocity);
    }

    public void StartMoving()
    {
        StartMovingRandomly();
    }

    public void StartMovingRandomly()
    {
        StartWait();
        CanMove = true;
    }

    public void InterruptMoving()
    {
        CanMove = false;
        isMovingSelf = false;

        MoveCompiler.RemoveSelfMoveFactor();
    }

    public void StartMove()
    {
        SetRandomMoveDirection();
        MoveTimeCounter = Random.Range(averageMoveTime * (1 - averageMoveTimeDelta), averageMoveTime * (1 + averageMoveTimeDelta));
        isMovingSelf = true;
    }

    public void StartWait()
    {
        TimeBetweenMovesCounter = Random.Range(betweenMovesAverageTime * (1 - betweenMovesAverageTimeDelta), betweenMovesAverageTime * (1 + betweenMovesAverageTimeDelta));
        isMovingSelf = false;

        MoveCompiler.RemoveSelfMoveFactor();
    }

    public void SetRandomMoveDirection()
    {
        switch (moveDirectionType)
        {
            case AutomoveDirectionType.ANY_DIRECTION:
                SetAnyDirectionRandomMoveDirection();
                break;

            case AutomoveDirectionType.FOUR_DIRECTIONS:
                SetFourDirectionsRandomMoveDirection();
                break;
        }
    }

    public void SetAnyDirectionRandomMoveDirection()
    {
        selfMove = new Vector3(Random.Range(-1f, 1f) * moveSpeed / 2, Random.Range(-1f, 1f) * moveSpeed / 2, 0);
    }

    public void SetFourDirectionsRandomMoveDirection()
    {
        int randomDirection = Random.Range(0, 4);

        switch (randomDirection)
        {
            case 0:
                selfMove = new Vector3(0, moveSpeed / 2, 0);
                break;

            case 1:
                selfMove = new Vector3(moveSpeed / 2, 0, 0);
                break;

            case 2:
                selfMove = new Vector3(0, -moveSpeed / 2, 0);
                break;

            case 3:
                selfMove = new Vector3(-moveSpeed / 2, 0, 0);
                break;
        }
    }
}
