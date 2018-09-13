using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IPersistent
{
    protected static bool gameManagerExists;

    // Use this for initialization
    void Start () {
        if (!gameManagerExists)
        {
            gameManagerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PauseGame(bool discreet = false)
    {
        // TODO : if not descreet, display a "pause" texts
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        // TODO : remove the "pause" text
        Time.timeScale = 1;
    }
}
