using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EntitiesOnStartCallbackManager_Callback();

public static class EntitiesOnStartCallbackManager {

    /* 
     * int = localInFileId
     * List<EntitiesOnStartCallbackManager_Callback> = list of callback functions to call
     */
    private static Dictionary<long, List<EntitiesOnStartCallbackManager_Callback>> entitiesOnStartCallbacks = new Dictionary<long, List<EntitiesOnStartCallbackManager_Callback>>();

    public static void Add(long localInFileId, EntitiesOnStartCallbackManager_Callback callback)
    {
        if (!entitiesOnStartCallbacks.ContainsKey(localInFileId))
        {
            entitiesOnStartCallbacks.Add(localInFileId, new List<EntitiesOnStartCallbackManager_Callback>());
        }

        entitiesOnStartCallbacks[localInFileId].Add(callback);
    }

    public static void Call(long localInFileId)
    {
        foreach (EntitiesOnStartCallbackManager_Callback callback in entitiesOnStartCallbacks[localInFileId])
        {
            callback();
        }

        entitiesOnStartCallbacks.Remove(localInFileId); // removes the callbacks list once they've been called
    }

    public static void CallAll()
    {
        foreach (KeyValuePair<long, List<EntitiesOnStartCallbackManager_Callback>> entityOnStartCallbacks in entitiesOnStartCallbacks)
        {
            foreach (EntitiesOnStartCallbackManager_Callback callback in entityOnStartCallbacks.Value)
            {
                callback();
            }
        }

        Clear();
    }

    public static void Clear()
    {
        entitiesOnStartCallbacks.Clear();
    }
}
