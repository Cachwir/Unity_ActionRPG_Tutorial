using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public int currentLevel;
    public int currentExp;
    public int[] xpToLevelUp;
    public int[] hpLevels;
    public int[] strengthLevels;
    public int[] defenseLevels;

    public int CurrentHp { get; set; }
    public int CurrentStrength { get; set; }
    public int CurrentDefense { get; set; }

    protected PlayerHealthManager playerHealthManager;

    // Use this for initialization
    void Start () {
        playerHealthManager = FindObjectOfType<PlayerHealthManager>();
        CurrentHp = hpLevels[1];
        CurrentStrength = strengthLevels[1];
        CurrentDefense = defenseLevels[1];
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (currentExp >= xpToLevelUp[currentLevel])
        {
            LevelUp();
        }
    }

    public void AddExperience(int amount)
    {
        currentExp += amount;
    }

    public void LevelUp()
    {
        currentLevel++;
        CurrentHp = hpLevels[currentLevel];
        CurrentStrength = strengthLevels[currentLevel];
        CurrentDefense = defenseLevels[currentLevel];

        playerHealthManager.PlayerCurrentHealth += CurrentHp - hpLevels[currentLevel - 1];
        playerHealthManager.playerMaxHealth = CurrentHp;
    }
}
