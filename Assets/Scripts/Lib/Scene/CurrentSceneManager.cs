using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CurrentSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

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

    public void LoadSceneFromId(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
