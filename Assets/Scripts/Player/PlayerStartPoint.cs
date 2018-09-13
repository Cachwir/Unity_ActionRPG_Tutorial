using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartPoint : MonoBehaviour {

    public string pointName;
    public Vector2 playerFacingDirection;

    private PlayerController mainPlayer;
    private CameraController mainCamera;

	// Use this for initialization
	void Start () {
        mainPlayer = FindObjectOfType<PlayerController>();
        mainCamera = FindObjectOfType<CameraController>();

        if (mainPlayer.StartPoint == pointName)
        {
            mainPlayer.LastMove = playerFacingDirection;
            mainPlayer.transform.position = this.transform.position;
            mainCamera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, mainCamera.transform.position.z);
            mainPlayer.StartPoint = null;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
