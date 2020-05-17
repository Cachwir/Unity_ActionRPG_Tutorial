using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SelfMovable, IPersistent
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

    protected new void FixedUpdate()
    {
        if (!IsInCinematicMode)
        {
            MovementsUpdate();
        }
        
        AttacksUpdate();
        base.FixedUpdate();
    }

    protected void MovementsUpdate()
    {
        Vector2 playerMovement = Vector2.zero;

        if (!DialogManager.isReading && !AreMovementsRestrained && !IsImmobilized && !IsPlayerRestrained && !IsAttacking)
        {
            playerMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }

        if (playerMovement != Vector2.zero)
        {
            selfMove = new Vector2(playerMovement.x * moveSpeed, playerMovement.y * moveSpeed);
            isMovingSelf = true;
        }
        else
        {
            StopMoving();
            selfMove = Vector2.zero;
            isMovingSelf = false;
        }
    }

    protected void AttacksUpdate()
    {
        if (!DialogManager.isReading && !IsInCinematicMode && !AreActionsRestrained && !IsPlayerRestrained && !IsAttacking)
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
