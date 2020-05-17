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
    protected Movable thisMovable;
    protected string moveId;

    // Use this for initialization
    void Awake()
    {
        thisMovable = GetComponent<Movable>();
    }

    // Update is called once per frame
    void Update ()
    {

    }

    void FixedUpdate()
    {
        HandleMovesOnUpdate();
    }

    protected void SetMove(Vector3 move)
    {
        if (thisMovable == null)
        {
            gameObject.transform.position = move;
        }
        else
        {
            move = Helper.MoveToVelocity(move, gameObject);
            moveId = thisMovable.MoveCompiler.AddOrEditMoveFactor(Helper.GetObjectLocalIdInFile(this), move, moveId, true, true);
        }
    }

    protected void RemoveMove()
    {
        if (thisMovable != null)
        {
            thisMovable.MoveCompiler.RemoveMoveFactor(Helper.GetObjectLocalIdInFile(this), moveId);
        }
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
                    SetMove(Vector3.Lerp(transform.position, targetDestination.transform.position, step));
                }
                else if (moveType == MoveType.LINEAR)
                {
                    SetMove(Vector3.MoveTowards(transform.position, targetDestination.transform.position, step));
                }
                else if (moveType == MoveType.EASEINOUT)
                {
                    SetMove(Vector3.Lerp(transform.position, targetDestination.transform.position, Mathf.SmoothStep(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, Time.fixedTime - moveStartFixedTime)) * (moveSpeed / 10)));
                }
            }
            else
            {
                HasFinishedMoving = true;
                RemoveMove();
                
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
            
        isMoving = true;
        moveStartFixedTime = Time.fixedTime;
    }
}
