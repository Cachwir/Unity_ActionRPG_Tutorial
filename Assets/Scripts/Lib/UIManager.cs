using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IPersistent
{
    public Slider healthBar;
    public Text hpText;
    public Text lvlText;
    public PlayerHealthManager playerHealthManager;

    static private bool uiExists = false;

    protected PlayerStats playerStats;

    // Use this for initialization
    void Start ()
    {
        playerStats = FindObjectOfType<PlayerStats>();

        if (!uiExists)
        {
            uiExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
        healthBar.maxValue = playerHealthManager.playerMaxHealth;
        healthBar.value = playerHealthManager.PlayerCurrentHealth;
        hpText.text = "HP: " + playerHealthManager.PlayerCurrentHealth + "/" + playerHealthManager.playerMaxHealth;
        lvlText.text = "Lvl:" + playerStats.currentLevel;
    }
}
