using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicStarter : MonoBehaviour {

    public Cinematic cinematic;

    protected CinematicsManager cinematicsManager;
    protected bool hasTriggeredEnter2D;

	// Use this for initialization
	void Start () {
        cinematicsManager = FindObjectOfType<CinematicsManager>();
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasTriggeredEnter2D)
        {
            hasTriggeredEnter2D = true;
            // It's important to start the cinematic from the CinematicsManagers as it will make the game enter the cinematic state
            cinematicsManager.StartCinematic(cinematic);
        }
    }
}
