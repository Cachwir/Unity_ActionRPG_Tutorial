using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableCinematicObjectController : MonoBehaviour {

    protected Movable movableGameObject;
    public bool AreMovesAnimated { get; set; }

    public const string DIRECTION_TOP = "top";
    public const string DIRECTION_DOWN = "down";
    public const string DIRECTION_LEFT = "left";
    public const string DIRECTION_RIGHT = "right";

    protected Vector3 previousPosition;

    // Use this for initialization
    void Awake () {
        movableGameObject = GetComponent<Movable>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        HandleHeadingDirectionOnUpdate();
    }

    public void HandleHeadingDirectionOnUpdate()
    {
        if (AreMovesAnimated)
        {
            movableGameObject.IsMoving = true;
            movableGameObject.MoveInput = (transform.position - previousPosition).normalized;
            movableGameObject.LastMove = movableGameObject.MoveInput;
        }
        else
        {
            movableGameObject.IsMoving = false;
        }

        previousPosition = transform.position;
    }

    public void SetLookDirection(string direction)
    {
        Vector2 Direction;

        switch (direction)
        {
            case DIRECTION_TOP:
                Direction = new Vector2(0, 1);
                break;

            case DIRECTION_DOWN:
                Direction = new Vector2(0, -1);
                break;

            case DIRECTION_LEFT:
                Direction = new Vector2(-1, 0);
                break;

            case DIRECTION_RIGHT:
                Direction = new Vector2(1, 0);
                break;

            default:
                throw new System.Exception("Wrong argument "+ direction +" passed to lookDirection.");
        }

        movableGameObject.LastMove = Direction;
    }

    public void AnimateMoves()
    {
        AreMovesAnimated = true;
        movableGameObject.IsInCinematicMode = true;
    }

    public void StopAnimatingMoves()
    {
        AreMovesAnimated = false;
        movableGameObject.IsInCinematicMode = false;
        movableGameObject.IsMoving = false;
    }
}
