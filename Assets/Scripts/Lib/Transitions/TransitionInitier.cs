using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionInitier : MonoBehaviour {

    public string levelToLoad;
    public string targetPointName;
    public Vector2 expectedFacingDirection;

    protected TransitionManager transitionManager;
    protected PlayerController mainPlayer;

    // Use this for initialization
    void Start () {
        mainPlayer = FindObjectOfType<PlayerController>();
        transitionManager = FindObjectOfType<TransitionManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown("space") && other.gameObject.name == "Player" && Helper.IsV2Equal(mainPlayer.LastMove, expectedFacingDirection))
        {
            transitionManager.TransitionToLevel(levelToLoad, targetPointName);
        }
    }
}
