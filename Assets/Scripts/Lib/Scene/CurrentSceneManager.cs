using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentSceneManager : MonoBehaviour, ICustomSceneLoadHandler
{
    protected GameManager gameManager;
    protected TransitionEffectManager transitionEffectManager;

    public Scene NextScene { get; set; }

    // Use this for initialization
    void Start ()
    {
        gameManager = FindObjectOfType<GameManager>();
        transitionEffectManager = FindObjectOfType<TransitionEffectManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Scene GetCurrentScene()
    {
        return SceneManager.GetActiveScene();
    }

    public List<MonoBehaviour> GetTransformSaveHolders()
    {
        List<MonoBehaviour> transformSaveHolders = new List<MonoBehaviour>();

        foreach (MonoBehaviour instanciatedObjects in FindObjectsOfType<MonoBehaviour>())
        {
            if (instanciatedObjects is ITransformSaveHolder)
            {
                transformSaveHolders.Add(instanciatedObjects);
            }
        }

        return transformSaveHolders;
    }

    public Movable[] GetMovableObjects()
    {
        return FindObjectsOfType<Movable>();
    }

    public AutoMoving[] GetAutoMovingObjects()
    {
        return FindObjectsOfType<AutoMoving>();
    }

    public EnnemyController[] GetEnnemies()
    {
        EnnemyController[] ennemies = FindObjectsOfType<EnnemyController>();
        return ennemies;
    }

    public void LoadSceneFromId(int sceneId, bool playBeforeTransition = false, bool playAfterTransition = false)
    {
        LoadScene(sceneId, playBeforeTransition, playAfterTransition);
    }

    public void LoadSceneFromName(string sceneName, bool playBeforeTransition = false, bool playAfterTransition = false)
    {
        LoadScene(SceneUtility.GetBuildIndexByScenePath(Config.PATH_TO_SCENES + "/" + sceneName), playBeforeTransition, playAfterTransition);
    }

    public void LoadScene(int sceneBuildIndex, bool playBeforeTransition = false, bool playAfterTransition = false)
    {
        TransitionEffectManager_Callback loadSceneCall = delegate()
        {
            SceneManager.LoadScene(sceneBuildIndex);

            if (playAfterTransition)
            {
                transitionEffectManager.PlayOutEffect(delegate () { });
            }
        };

        if (playBeforeTransition)
        {
            transitionEffectManager.PlayInEffect(loadSceneCall);
        }
        else
        {
            loadSceneCall();
        }
    }

    public void SceneSaveLoadManager_LoadSceneFromId(int sceneId)
    {
        LoadSceneFromId(sceneId, true, true);
    }
}
