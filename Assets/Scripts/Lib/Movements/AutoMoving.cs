using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoving : Movable {

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

        MoveTimeCounter = Random.Range(averageMoveTime * 0.75f, averageMoveTime * 1.25f);
        TimeBetweenMovesCounter = Random.Range(betweenMovesAverageTime * 0.75f, betweenMovesAverageTime * 1.25f);

        StartMovingRandomly();
    }

    // Update is called once per frame
    protected new void Update ()
    {
        base.Update();
	}

    protected void FixedUpdate()
    {
        HandleMovesOnUpdate();
    }

    public void HandleMovesOnUpdate()
    {
        if (CanMove)
        {
            if (!IsInCinematicMode && IsMoving && AdditionalMovingConditions())
            {
                MoveTimeCounter -= Time.fixedDeltaTime;

                if (AreMovementsRestrained)
                {
                    thisRigidbody.velocity = Vector2.zero;
                }
                else
                {
                    thisRigidbody.velocity = LastMove;
                }

                if (MoveTimeCounter <= 0)
                {
                    StartWait();
                }
            }
            else
            {
                TimeBetweenMovesCounter -= Time.fixedDeltaTime;

                if (TimeBetweenMovesCounter <= 0)
                {
                    StartMove();
                }
            }
        }
    }

    protected virtual bool AdditionalMovingConditions()
    {
        return true;
    }

    public void StartMovingRandomly()
    {
        CanMove = true;
        StartWait();
    }

    public void InterruptMoving()
    {
        CanMove = false;
        IsMoving = false;
        thisRigidbody.velocity = Vector2.zero;
    }

    public void StartMove()
    {
        SetRandomMoveDirection();
        MoveTimeCounter = Random.Range(averageMoveTime * (1 - averageMoveTimeDelta), averageMoveTime * (1 + averageMoveTimeDelta));
        IsMoving = true;
    }

    public void StartWait()
    {
        TimeBetweenMovesCounter = Random.Range(betweenMovesAverageTime * (1 - betweenMovesAverageTimeDelta), betweenMovesAverageTime * (1 + betweenMovesAverageTimeDelta));
        IsMoving = false;
        thisRigidbody.velocity = Vector2.zero;
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
        LastMove = new Vector2(Random.Range(-1f, 1f) * moveSpeed / 2, Random.Range(-1f, 1f) * moveSpeed / 2);
        MoveInput = LastMove;
    }

    public void SetFourDirectionsRandomMoveDirection()
    {
        int randomDirection = Random.Range(0, 4);

        switch (randomDirection)
        {
            case 0:
                LastMove = new Vector2(0, moveSpeed / 2);
                break;

            case 1:
                LastMove = new Vector2(moveSpeed / 2, 0);
                break;

            case 2:
                LastMove = new Vector2(0, -moveSpeed / 2);
                break;

            case 3:
                LastMove = new Vector2(-moveSpeed / 2, 0);
                break;
        }

        MoveInput = LastMove;
    }
}
