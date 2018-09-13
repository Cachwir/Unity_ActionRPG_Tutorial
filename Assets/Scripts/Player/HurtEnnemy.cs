using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnnemy : MonoBehaviour {

    public int baseDamageAmount;
    public GameObject hurtEffect;
    public GameObject hitPoint;
    public GameObject damageNumberText;

    protected GameObject player;
    protected PlayerStats playerStats;

    // Use this for initialization
    void Start () {
        player = this.transform.parent.gameObject;
        playerStats = FindObjectOfType<PlayerStats>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemies")
        {
            int damageAmount = baseDamageAmount + playerStats.CurrentStrength;
            other.gameObject.GetComponent<EnnemyHealthManager>().TryHurtEnnemy(this, damageAmount);
        }
    }

    public void PlayHurtEffect(EnnemyHealthManager ennemyHealthManager, int damageAmount)
    {
        Instantiate(hurtEffect, hitPoint.transform.position, hitPoint.transform.rotation);

        var damageNumberTextClone = (GameObject) Instantiate(damageNumberText, hitPoint.transform.position, Quaternion.Euler(Vector3.zero));
        damageNumberTextClone.GetComponent<FloatingText>().text = "" + damageAmount;

        if (ennemyHealthManager.IsDying())
        {
            ennemyHealthManager.PlayDeadSFX();
        }
        else
        {
            ennemyHealthManager.PlayHitSFX();
        }
    }
}
