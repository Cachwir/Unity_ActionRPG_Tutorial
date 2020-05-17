using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyHealthManager : MonoBehaviour {

    public int ennemyMaxHealth;
    public float immunityDuration = 0.11f;
    public int xpToGive;

    protected float immunityDurationCounter;
    protected bool isSufferingDamage;
    protected PlayerStats playerStats;
    protected SFXController sfxController;
    protected EnnemyController ennemyController;

    protected EventSubscriber eventSubscriber = new EventSubscriber();
    protected string ennemyKilledEvent = "ennemyKilled";

    // Use this for initialization
    void Start()
    {
        EnnemyCurrentHealth = ennemyMaxHealth;
        immunityDurationCounter = immunityDuration;
        playerStats = FindObjectOfType<PlayerStats>();
        sfxController = FindObjectOfType<SFXController>();
        ennemyController = GetComponent<EnnemyController>();
    }

    public int EnnemyCurrentHealth { get; set; }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (IsDying())
        {
            playerStats.AddExperience(xpToGive);
            eventSubscriber.Trigger(ennemyKilledEvent);
            Destroy(this.gameObject);
        }

        if (isSufferingDamage)
        {
            if (immunityDurationCounter > 0)
            {
                immunityDurationCounter -= Time.fixedDeltaTime;
            }
            else
            {
                isSufferingDamage = false;
            }
        }
    }

    public bool IsDying()
    {
        return EnnemyCurrentHealth <= 0;
    }

    public void TryHurtEnnemy(HurtEnnemy weapon, int damageAmount)
    {
        if (!isSufferingDamage)
        {
            isSufferingDamage = true;
            immunityDurationCounter = immunityDuration;
            HurtEnnemy(damageAmount);
            weapon.PlayHurtEffect(this, damageAmount);
        }
    }

    public void HurtEnnemy(int damageAmount)
    {
        EnnemyCurrentHealth -= damageAmount;
        ennemyController.EnterCombat();
    }

    public void HealEnnemy(int amount)
    {
        if (EnnemyCurrentHealth + amount >= ennemyMaxHealth)
        {
            EnnemyCurrentHealth = ennemyMaxHealth;
        }
        else
        {
            EnnemyCurrentHealth += amount;
        }
    }

    public void FullyHealEnnemy()
    {
        EnnemyCurrentHealth = ennemyMaxHealth;
    }

    public void SubscribeEnnemyKilledEvent(EventSubscriber_EventCallback callback)
    {
        eventSubscriber.Subscribe(ennemyKilledEvent, callback);
    }

    public void PlayHitSFX()
    {
        sfxController.PlaySoundEffect("cuteCreatureHit");
    }

    public void PlayDeadSFX()
    {
        sfxController.PlaySoundEffect("cuteCreatureDeath");
    }
}
