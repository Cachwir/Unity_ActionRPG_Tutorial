using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public Scene GetCurrentScene()
    {
        return SceneManager.GetActiveScene();
    }

    public void DeactivateCurrentSceneRunningElements()
    {
        foreach (IGameLoadListener gameLoadListener in FindObjectsOfType<MonoBehaviour>().OfType<IGameLoadListener>())
        {
            gameLoadListener.OnGameLoad();
        }
    }

    public void DeactivateCurrentSceneUnpersistentObjects()
    {
        SaveLoadManagerHelper helper = FindObjectOfType<SaveLoadManagerHelper>();

        foreach (MonoBehaviour monoObject in FindObjectsOfType<MonoBehaviour>())
        {
            if (!helper.IsPersistent(monoObject.gameObject))
            {
                monoObject.gameObject.SetActive(false);
            }
        }
    }
}
