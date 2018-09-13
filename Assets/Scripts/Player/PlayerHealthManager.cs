using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour {

    public int playerMaxHealth;
    public float blinkDuration = 0.5f;
    public float timeBetweenEachBlink = 0.05f;

    public bool IsBlinking { get; set; }
    protected float numberOfBlinks;

    protected SpriteRenderer playerSpriteRenderer;
    protected SFXController sfxController;

    // Use this for initialization
    void Start () {
        PlayerCurrentHealth = playerMaxHealth;
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        numberOfBlinks = (int) (blinkDuration / timeBetweenEachBlink);
        sfxController = FindObjectOfType<SFXController>();
    }

    public int PlayerCurrentHealth { get; set; }

    // Update is called once per frame
    void Update () {
        HandlePlayerCurrentHealthUpdate();
    }

    public void HandlePlayerCurrentHealthUpdate()
    {
        if (IsDying())
        {
            this.gameObject.SetActive(false);
        }
    }

    public bool IsDying()
    {
        return PlayerCurrentHealth <= 0;
    }

    public void HurtPlayer(int amount)
    {
        PlayerCurrentHealth -= amount;
    }

    public void HealPlayer(int amount)
    {
        if (PlayerCurrentHealth + amount >= playerMaxHealth)
        {
            PlayerCurrentHealth = playerMaxHealth;
        }
        else
        {
            PlayerCurrentHealth += amount;
        }
    }

    public void FullyHealPlayer()
    {
        PlayerCurrentHealth = playerMaxHealth;
    }

    public bool CanTakeDamage()
    {
        return !DialogManager.isReading && !IsBlinking;
    }

    public void Blink()
    {
        IsBlinking = true;
        StartCoroutine("StartBlinking");
    }

    public void StopBlinking()
    {
        IsBlinking = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    IEnumerator StartBlinking()
    {
        for (int i = 0; i < numberOfBlinks; i++)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.3f); //Red, Green, Blue, Alpha/Transparency
            yield return new WaitForSeconds(timeBetweenEachBlink);
            GetComponent<SpriteRenderer>().color = Color.white; //White is the default "color" for the sprite, if you're curious.
            yield return new WaitForSeconds(timeBetweenEachBlink);
        }

        StopBlinking();

    } //This IEnumerator runs 3 times, resulting in 3 flashes.﻿

    public void PlayHitSFX()
    {
        sfxController.PlaySoundEffect("playerHurt");
    }

    public void PlayDeadSFX()
    {
        sfxController.PlaySoundEffect("playerHurt");
    }
}
