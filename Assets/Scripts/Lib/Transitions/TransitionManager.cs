using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    protected PlayerController mainPlayer;
    protected CurrentSceneManager currentSceneManager;
    protected CombatManager combatManager;

    // Use this for initialization
    void Start()
    {
        mainPlayer = FindObjectOfType<PlayerController>();
        currentSceneManager = FindObjectOfType<CurrentSceneManager>();
    }

    public void TransitionToLevel(string levelToLoad, string targetPointName)
    {
        CallObservedOnLevelUnloadComponents();

        mainPlayer.StartPoint = targetPointName;
        currentSceneManager.LoadSceneFromName(levelToLoad, true, true);
    }

    protected void CallObservedOnLevelUnloadComponents()
    {
        foreach (IOnLevelUnload levelUnloadListener in FindObjectsOfType<MonoBehaviour>().OfType<IOnLevelUnload>())
        {
            levelUnloadListener.OnLevelUnload();
        }
    }
}
