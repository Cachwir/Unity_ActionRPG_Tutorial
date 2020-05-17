using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class EnnemyController : AutoMoving, IPointerClickHandler
{
    public List<EnnemyInFightMovementsPattern> ennemyInFightMovementsPatterns;

    protected CombatManager combatManager;
    protected SFXController sfxController;
    protected EnnemyAttacksManager ennemyAttacksManager;
    protected EnnemyCombatController ennemyCombatController;

    public bool IsInCombat { get; set; }

    void Awake()
    {
        combatManager = FindObjectOfType<CombatManager>();
        combatManager.RegisterEnnemy(this);
    }

    // Use this for initialization
    new void Start()
    {
        base.Start();

        sfxController = FindObjectOfType<SFXController>();
        ennemyAttacksManager = GetComponent<EnnemyAttacksManager>();
        ennemyCombatController = GetComponent<EnnemyCombatController>();
    }
	
	// Update is called once per frame
	new void Update ()
    {
        base.Update();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        sfxController.PlaySoundEffect("cuteCreatureHit");
    }

    public void EnterCombat()
    {
        if (!IsInCombat)
        {
            IsInCombat = true;
            combatManager.NotifyEnterCombat(this);
            ennemyAttacksManager.StartUsingAttacks();
        }
    }

    public void LeaveCombat()
    {
        if (IsInCombat)
        {
            IsInCombat = false;
            ennemyCombatController.StopMovement();
            ennemyAttacksManager.StopUsingAttacks();
            moveSpeed = defaultMoveSpeed;
        }
    }
}
