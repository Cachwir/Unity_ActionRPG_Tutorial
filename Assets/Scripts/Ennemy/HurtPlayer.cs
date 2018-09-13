using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour {

    public int baseDamageAmount;
    public GameObject damageNumberText;

    protected PlayerStats playerStats;

    // Use this for initialization
    void Start () {
        playerStats = FindObjectOfType<PlayerStats>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetDamageAmount()
    {
        return baseDamageAmount - playerStats.CurrentDefense;
    }

    protected virtual void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.name == "Player" && other.gameObject.GetComponent<PlayerHealthManager>().CanTakeDamage())
        {
            int damageAmount = GetDamageAmount();

            other.gameObject.GetComponent<PlayerHealthManager>().HurtPlayer(damageAmount);
            PlayHurtEffect(other.gameObject, damageAmount);
        }
    }

    public void PlayHurtEffect(GameObject other, int damageAmount)
    {
        var damageNumberTextClone = (GameObject)Instantiate(damageNumberText, other.transform.position, Quaternion.Euler(Vector3.zero));
        damageNumberTextClone.GetComponent<FloatingText>().text = "" + damageAmount;

        PlayerHealthManager playerHealthManager = other.GetComponent<PlayerHealthManager>();

        playerHealthManager.Blink();

        if (playerHealthManager.IsDying())
        {
            playerHealthManager.PlayDeadSFX();
        }
        else
        {
            playerHealthManager.PlayHitSFX();
        }
    }
}
