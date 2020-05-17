using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour, ITransformSaveHolder
{
    // MOVE
    public float moveSpeed = 1;
    public bool is2D = true;

    public bool IsMoving { get; set; }
    public Vector2 LastMove { get; set; }
    public bool AreMovementsRestrained { get; set; } // blocks movements from this Movable
    public bool IsImmobilized { get; set; } // blocks movements from any source
    public Vector2 MoveInput { get; set; }
    public bool IsInCinematicMode { get; set; } // if true, blocks all movements except from a cinematic source (instance of CinematicObjectMove)

    protected Animator animator;
    protected Helper helper;
    private Rigidbody2D thisRigidbody;
    public MoveCompiler MoveCompiler { get; private set; } // The move compiler handles the compilation of all move forces affected to this object and returns a clean compiled velocity
    protected float defaultAnimationsSpeed = 1;
    protected float defaultMoveSpeed;

    // Use this for initialization
    protected void Start ()
    {
        animator = GetComponent<Animator>();
        thisRigidbody = GetComponent<Rigidbody2D>();
        helper = FindObjectOfType<Helper>();
        LastMove = new Vector2(0, -1);
        MoveCompiler = new MoveCompiler(this, helper);
        defaultMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    protected void Update ()
    {
        if (animator != null)
        {
            animator.SetFloat("moveX", MoveInput.x);
            animator.SetFloat("moveY", MoveInput.y);

            animator.SetFloat("lastMoveX", LastMove.x);
            animator.SetFloat("lastMoveY", LastMove.y);

            animator.SetBool("isMoving", IsMoving);
        }
    }

    protected void LateUpdate()
    {
        thisRigidbody.velocity = MoveCompiler.Compile();
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void RestoreDefaultMoveSpeed(float speed)
    {
        moveSpeed = defaultMoveSpeed;
    }

    public long GetLocalIdInFile()
    {
        return Helper.GetObjectLocalIdInFile(this);
    }

    public void RestrainMovements()
    {
        AreMovementsRestrained = true;
    }

    public void UnrestrainMovements()
    {
        AreMovementsRestrained = false;
    }

    public void Immobilize()
    {
        IsImmobilized = true;
    }

    public void Unimmobilize()
    {
        IsImmobilized = false;
    }

    public void EnterCinematicMode()
    {
        IsInCinematicMode = true;
        StopMoving();
    }

    public void LeaveCinematicMode()
    {
        IsInCinematicMode = false;
    }

    protected virtual void StopMoving()
    {
        MoveCompiler.RemoveSelfMoveFactor();
    }

    public virtual bool ValidateMoveDirection(Vector3 velocity)
    {
        return true;
    }

    public void SetAnimationSpeed(float speed)
    {
        animator.speed = speed;
    }

    public void ResetAnimationsSpeed()
    {
        animator.speed = defaultAnimationsSpeed;
    }
}
