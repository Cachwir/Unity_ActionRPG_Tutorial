using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBounds : MonoBehaviour {

    private BoxCollider2D thisBoxCollider2D;
    private CameraController cameraController;

	// Use this for initialization
	void Start () {
        thisBoxCollider2D = GetComponent<BoxCollider2D>();
        cameraController = FindObjectOfType<CameraController>();

        cameraController.SetMapBounds(thisBoxCollider2D);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
