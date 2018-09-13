using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public delegate void SaveLoadManager_OnSceneLoaded();

public class SaveLoadManager : MonoBehaviour, IPersistent {

    public List<IEntitySaveLoadManager> entitySaveLoadManagers = new List<IEntitySaveLoadManager>();

    protected static bool gameManagerExists;
    public List<SaveLoadManager_OnSceneLoaded> OnSceneLoadedOnceCallbacks { get; set; }

    // called before Start, it's here to add OnSceneLoaded to the sceneLoaded events (handled by Unity)
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (OnSceneLoadedOnceCallbacks == null)
        {
            OnSceneLoadedOnceCallbacks = new List<SaveLoadManager_OnSceneLoaded>();
        }

        if (OnSceneLoadedOnceCallbacks.Count > 0)
        {
            foreach (SaveLoadManager_OnSceneLoaded callback in OnSceneLoadedOnceCallbacks)
            {
                callback();
            }

            OnSceneLoadedOnceCallbacks.Clear();
        }
    }

    private void Start()
    {
        Init();
    }

    public void AddOnSceneLoadCallback(SaveLoadManager_OnSceneLoaded callback)
    {
        OnSceneLoadedOnceCallbacks.Add(callback);
    }

    public void Init()
    {
        foreach (Transform child in transform)
        {
            switch(child.name)
            {
                case "SceneSaveLoadManager":
                    // First, let's add the SceneSaveLoadManager
                    entitySaveLoadManagers.Add(child.GetComponent<IEntitySaveLoadManager>());
                    break;

                case "CustomManagers":
                    // Then, let's add all the custom SaveLoadManagers
                    foreach (Transform customManager in child)
                    {
                        entitySaveLoadManagers.Add(customManager.GetComponent<IEntitySaveLoadManager>());
                    }
                    break;
            }
        }
    }

    public void Save()
    {
        PauseGame();

        foreach (IEntitySaveLoadManager entitySaveLoadManager in entitySaveLoadManagers)
        {
            entitySaveLoadManager.Save();
        }

        UnpauseGame();
    }

    public void Load()
    {
        PauseGame();

        foreach (IEntitySaveLoadManager entitySaveLoadManager in entitySaveLoadManagers)
        {
            entitySaveLoadManager.Load();
        }

        UnpauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }
}
