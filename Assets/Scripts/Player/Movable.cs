using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour, ITransformSaveHolder
{
    // MOVE
    public float moveSpeed = 1;

    protected Animator animator;
    public bool IsMoving { get; set; }
    protected Rigidbody2D thisRigidbody;
    public Vector2 LastMove { get; set; }
    public bool AreMovementsRestrained { get; set; } // movements
    public Vector2 MoveInput { get; set; }
    public bool IsInCinematicMode { get; set; } // if true, deactivates this script's moves handlings and lets the cinematic module handle them

    // Use this for initialization
    protected void Start () {
        animator = GetComponent<Animator>();
        thisRigidbody = GetComponent<Rigidbody2D>();
        LastMove = new Vector2(0, -1);
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

    public void RestrainMovements()
    {
        AreMovementsRestrained = true;
    }

    public void UnrestrainMovements()
    {
        AreMovementsRestrained = false;
    }
}
