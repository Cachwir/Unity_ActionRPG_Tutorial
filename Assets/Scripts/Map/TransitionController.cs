using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour {

    public string levelToLoad;
    public string targetPointName;
    public Vector2 expectedFacingDirection;

    private PlayerController mainPlayer;

    // Use this for initialization
    void Start () {
        mainPlayer = FindObjectOfType<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown("space") && other.gameObject.name == "Player" && Helper.IsV2Equal(mainPlayer.LastMove, expectedFacingDirection))
        {
            mainPlayer.StartPoint = targetPointName;
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
