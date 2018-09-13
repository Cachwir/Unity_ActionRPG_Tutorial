using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCameraMoveBuilder : AbstractCinematicObjectMoveBuilder
{
    public Camera targetCamera; // optionnal

    void Start()
    {
        CameraController cameraController = FindObjectOfType<CameraController>();

        if (targetCamera == null)
        {
            target = cameraController.gameObject;
            targetDestination.transform.position = new Vector3(targetDestination.transform.position.x - cameraController.CameraHalfWidth, targetDestination.transform.position.y - cameraController.CameraHalfHeight, cameraController.transform.position.z);
        }
        else
        {
            target = targetCamera.gameObject;
        }

        _zPositionFixedValue = target.transform.position.z;

        // TODO position the camera appropriately if it's not the main camera

        // Clamps the targetDestination so the camera doesn't infinitely try to reach it, hitting the level borders
        targetDestination.transform.position = cameraController.GetClampedPosition(targetDestination.transform.position);
    }
}
