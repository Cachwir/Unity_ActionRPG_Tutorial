using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Movable, IPersistent
{
    // INIT
    public string StartPoint { get; set; }

    // MOVE
    public bool AreActionsRestrained { get; set; } // attack
    public bool IsPlayerRestrained { get; set; } // attack + movements

    // Attack
    public bool IsAttacking { get; set; }
    public float attackDuration;
    private float attackDurationCounter;
    private SFXController sfxController;

    static private bool playerExists = false;

    // Use this for initialization
    new void Start () {
        base.Start();

        if (!playerExists) {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        } else {
            Destroy(this.gameObject);
        }

        this.LastMove = new Vector2(0, -1);
        this.sfxController = FindObjectOfType<SFXController>();
        this.AreActionsRestrained = false;
        this.IsPlayerRestrained = false;
    }

    // Update is called once per frame
    new void Update ()
    {
        base.Update();
        animator.SetBool("isAttacking", IsAttacking);
    }

    private void FixedUpdate()
    {
        if (!IsInCinematicMode)
        {
            MovementsUpdate();
            AttacksUpdate();
        }
    }

    protected void MovementsUpdate()
    {
        this.IsMoving = false;
        MoveInput = Vector2.zero;

        if (!DialogManager.isReading && !AreMovementsRestrained && !IsPlayerRestrained && !IsAttacking)
        {
            MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }

        if (MoveInput != Vector2.zero)
        {
            thisRigidbody.velocity = new Vector2(MoveInput.x * moveSpeed, MoveInput.y * moveSpeed);
            IsMoving = true;
            LastMove = MoveInput;
        }
        else
        {
            thisRigidbody.velocity = Vector2.zero;
            IsMoving = false;
        }
    }

    protected void AttacksUpdate()
    {
        if (!DialogManager.isReading && !AreActionsRestrained && !IsPlayerRestrained && !IsAttacking)
        {
           if (Input.GetKeyDown(KeyCode.C))
            {
                attackDurationCounter = attackDuration;
                IsAttacking = true;
                this.sfxController.PlaySoundEffect("swordSlash");
            }
        }
        else
        {
            if (attackDurationCounter > 0)
            {
                attackDurationCounter -= Time.fixedDeltaTime;
            }
            else
            {
                IsAttacking = false;
            }
        }
    }

    public void RestrainActions()
    {
        AreActionsRestrained = true;
    }

    public void UnrestrainActions()
    {
        AreActionsRestrained = false;
    }

    public void RestrainPlayer()
    {
        IsPlayerRestrained = true;
    }

    public void UnrestrainPlayer()
    {
        IsPlayerRestrained = false;
    }
}
