using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

abstract public class AbstractSaveLoadManager : MonoBehaviour, IEntitySaveLoadManager
{
    public string fileName = "make_this_unique.sav";

    protected MonoBehaviour entityManager; // needs to be defined in the overriden Start function
    protected SaveLoadManager saveLoadManager;
    protected string currentSceneFilePath;

    protected virtual void Start()
    {
        saveLoadManager = FindObjectOfType<SaveLoadManager>();
        currentSceneFilePath = Application.persistentDataPath + "/" + fileName; // Application.persistentDataPath doesn't work on Web.
    }

    public void Save()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter(); // selects the type of data formatting algorithm (here, binary)
        FileStream fileStream = new FileStream(currentSceneFilePath, FileMode.Create); // creates and opens the save file

        ObjectData currentSceneData = GetObjectData(entityManager); // prepares the data for serialization

        binaryFormatter.Serialize(fileStream, currentSceneData); // serializes and saves the data in the file
        fileStream.Close(); // closes the stream (forget this and it will throw an error the next time you try to open it
    }

    public void Load()
    {
        if (SaveFileExists())
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(currentSceneFilePath, FileMode.Open);

            ObjectData currentSceneData = binaryFormatter.Deserialize(fileStream) as ObjectData; // deserializes the data and casts the type SceneData to tell what their class is. You could also use (PlayerData) binaryFormatter.Deserialize(fileStream).
            fileStream.Close();

            RebuildFromData(currentSceneData);
        }
    }

    public bool SaveFileExists()
    {
        return File.Exists(currentSceneFilePath);
    }

    // Executes the given callback on scene load, this is generally where you'll call PrepareDataForEntity
    public void CallAfterSceneLoaded(SaveLoadManager_OnSceneLoaded callback)
    {
        saveLoadManager.AddOnSceneLoadCallback(callback);
    }

    // sets the data if target data manager (or entity manager) is persistent (implemented IPersistent). Otherwise, stores it for it to be called after that entity's Start function
    public void PrepareDataForEntity(MonoBehaviour manager, EntitiesOnStartCallbackManager_Callback setData)
    {
        SaveLoadManagerHelper helper = FindObjectOfType<SaveLoadManagerHelper>();

        if (helper.IsPersistent(manager.gameObject))
        {
            setData();
        }
        else
        {
            EntitiesOnStartCallbackManager.Add(SaveLoadManagerHelper.GetObjectLocalIdInFile(manager), setData);
        }
    }

    public abstract ObjectData GetObjectData(MonoBehaviour entityManager);

    public abstract void RebuildFromData(ObjectData cameraData);

    [Serializable]
    public abstract class ObjectData {}
}
