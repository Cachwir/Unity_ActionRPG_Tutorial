using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraSaveLoadManager : AbstractSaveLoadManager
{
    protected override void Start()
    {
        base.Start();
        entityManager = FindObjectOfType<CameraController>();
    }

    public override ObjectData GetObjectData(MonoBehaviour entityManager)
    {
        return new CameraData((CameraController) entityManager);
    }

    public override void RebuildFromData(ObjectData objectData)
    {
        CameraData cameraData = (CameraData) objectData;
        CameraController cameraController = (CameraController) entityManager;

        // call this after the scene has loaded
        CallAfterSceneLoaded(delegate ()
        {
            // call this to execute after the entity's Start function has been called (except for IPersistent entities)
            PrepareDataForEntity(cameraController, delegate ()
            {
                // cameraController's Start has been executed, so it's time to update its data
                cameraController.transform.position = new Vector3(
                    cameraData.position[0],
                    cameraData.position[1],
                    cameraData.position[2]
                    );
                cameraController.transform.rotation = new Quaternion(
                    cameraData.rotation[0],
                    cameraData.rotation[1],
                    cameraData.rotation[2],
                    cameraData.rotation[3]
                    );

                MonoBehaviour target = FindObjectOfType<Helper>().FindObjectByLocalIdInFile(cameraData.targetLocalIdInFile);

                if (target == null)
                {
                    throw new System.Exception("Cannot set camera's target as it couldn't be found through its targetLocalIdInFile : " + cameraData.targetLocalIdInFile);
                }

                cameraController.target = target;
                cameraController.IsManuallyControlled = cameraData.IsManuallyControlled;
                cameraController.moveSpeed = cameraData.moveSpeed;
            });
        });
    }

    [Serializable]
    public class CameraData : ObjectData
    {
        public float[] position = new float[3];
        public float[] rotation = new float[4];
        public long targetLocalIdInFile;
        public bool IsManuallyControlled;
        public float moveSpeed;

        public CameraData(CameraController cameraController)
        {
            position[0] = cameraController.transform.position.x;
            position[1] = cameraController.transform.position.y;
            position[2] = cameraController.transform.position.z;

            rotation[0] = cameraController.transform.rotation.x;
            rotation[1] = cameraController.transform.rotation.y;
            rotation[2] = cameraController.transform.rotation.z;
            rotation[3] = cameraController.transform.rotation.w;

            targetLocalIdInFile = Helper.GetObjectLocalIdInFile(cameraController.target);
            IsManuallyControlled = cameraController.IsManuallyControlled;
            moveSpeed = cameraController.moveSpeed;
        }
    }
}
