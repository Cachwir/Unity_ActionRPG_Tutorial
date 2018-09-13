using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CinematicObjectMove_Callback();

/**
 * Attach this Component to the gameObject you want to move
 */
public class CinematicObjectMove : MonoBehaviour {

    public enum MoveType
    {
        LINEAR,
        LERP,
        EASEINOUT
    };

    public float moveSpeed = 2.0f;
    public GameObject targetDestination;
    public Vector3 targetDestinationPosition;
    public bool isZPositionFixed = true; // If true, the z position of this gameObject will be fixed (in order to avoid some bugs)
    public float zPositionFixedValue = 0; // The value the z position will be fixed to
    public CinematicObjectMove_Callback afterMoveCallback = delegate() { };
    public MoveType moveType;
    
    protected bool isMoving;
    protected float moveStartFixedTime;
    public bool HasFinishedMoving { get; set; }
    protected MovableCinematicObjectController movableCinematicObjectController;

    // Use this for initialization
    void Awake()
    {
        // If the moved GameObject is animated, we assume it is of Movable class
        if (GetComponent<Movable>() != null)
        {
            // Let's attach the CinematicMovableObjectController to it
            gameObject.AddComponent<MovableCinematicObjectController>();
            movableCinematicObjectController = GetComponent<MovableCinematicObjectController>();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    void FixedUpdate()
    {
        HandleMovesOnUpdate();
    }

    public void HandleMovesOnUpdate()
    {
        if (IsMoving())
        {
            float step = Time.fixedDeltaTime * moveSpeed;

            if (Vector3.Distance(transform.position, targetDestination.transform.position) > 0.1f)
            {
                if (moveType == MoveType.LERP)
                {
                    transform.position = Vector3.Lerp(transform.position, targetDestination.transform.position, step);
                }
                else if (moveType == MoveType.LINEAR)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetDestination.transform.position, step);
                }
                else if (moveType == MoveType.EASEINOUT)
                {
                    transform.position = Vector3.Lerp(transform.position, targetDestination.transform.position, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, Time.fixedTime - moveStartFixedTime)) * (moveSpeed / 10));
                }

                if (movableCinematicObjectController != null)
                {
                    // Gets a vector that points from the cinematic object's position to the target's.
                    movableCinematicObjectController.HeadingDirection = targetDestination.transform.position - transform.position;
                }
            }
            else
            {
                HasFinishedMoving = true;

                if (movableCinematicObjectController != null)
                {
                    movableCinematicObjectController.StopAnimatingMoves();
                    Destroy(movableCinematicObjectController);
                }

                afterMoveCallback();
            }
        }
    }

    public bool IsMoving()
    {
        return isMoving && !HasFinishedMoving;
    }

    public void StartMoving()
    {
        if (isZPositionFixed)
        {
            targetDestinationPosition = new Vector3(targetDestination.transform.position.x, targetDestination.transform.position.y, zPositionFixedValue);
        }
        else
        {
            targetDestinationPosition = new Vector3(targetDestination.transform.position.x, targetDestination.transform.position.y, targetDestination.transform.position.z);
        }

        if (movableCinematicObjectController != null)
        {
            movableCinematicObjectController.HeadingDirection = targetDestination.transform.position - transform.position;
            movableCinematicObjectController.AnimateMoves();
        }
            
        isMoving = true;
        moveStartFixedTime = Time.fixedTime;
    }
}
