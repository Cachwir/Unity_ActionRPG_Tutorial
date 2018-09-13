using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusic : MonoBehaviour {

    public string trackSlug;

    protected MusicController musicController;

	// Use this for initialization
	void Start () {
        musicController = FindObjectOfType<MusicController>();
        musicController.SwitchToTrack(trackSlug);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
