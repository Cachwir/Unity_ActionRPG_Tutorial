using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSaveLoadManager : AbstractSaveLoadManager
{
    public GameObject OnStartCallbackCallerPrefab;

    protected override void Start()
    {
        base.Start();
        entityManager = FindObjectOfType<SceneLoadManager>();
    }

    public override ObjectData GetObjectData(MonoBehaviour entityManager)
    {
        return new SceneData((SceneLoadManager) entityManager);
    }

    public override void RebuildFromData(ObjectData objectData)
    {
        SceneLoadManager sceneLoadManager = (SceneLoadManager) entityManager;

        sceneLoadManager.DeactivateCurrentSceneRunningElements();
        sceneLoadManager.DeactivateCurrentSceneUnpersistentObjects();

        SceneData currentSceneData = (SceneData) objectData;
        SceneManager.LoadScene(currentSceneData.sceneId);

        CallAfterSceneLoaded(delegate ()
        {
            // adds the OnStartCallbackCaller, an object whose purpose is have the first Update() call, which will call all the delegated functions prepared by PrepareDataForEntity (for non persistent entities)
            Instantiate(OnStartCallbackCallerPrefab);
        });
    }

    [Serializable]
    public class SceneData : ObjectData
    {
        public int sceneId;

        public SceneData(SceneLoadManager sceneLoadManager)
        {
            sceneId = sceneLoadManager.GetCurrentScene().buildIndex;
        }
    }
}
