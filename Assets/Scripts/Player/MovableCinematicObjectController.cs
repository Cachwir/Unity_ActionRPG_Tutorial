using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableCinematicObjectController : MonoBehaviour {

    protected Movable movableGameObject;
    public Vector2 HeadingDirection { get; set; }
    public bool AreMovesAnimated { get; set; }

    public const string DIRECTION_TOP = "top";
    public const string DIRECTION_DOWN = "down";
    public const string DIRECTION_LEFT = "left";
    public const string DIRECTION_RIGHT = "right";

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
        if (AreMovesAnimated && HeadingDirection != Vector2.zero)
        {
            var distance = HeadingDirection.magnitude;
            var direction = HeadingDirection / distance; // This is now the normalized direction.

            movableGameObject.IsMoving = true;
            movableGameObject.MoveInput = direction;
        }
        else
        {
            movableGameObject.IsMoving = false;
        }
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
