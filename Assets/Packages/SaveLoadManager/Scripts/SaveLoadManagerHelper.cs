using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class SaveLoadManagerHelper : MonoBehaviour {

    static public long GetObjectLocalIdInFile(Object _object)
    {
        long idInFile = 0;
        SerializedObject serialize = new SerializedObject(_object);
        PropertyInfo inspectorModeInfo =
            typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);
        if (inspectorModeInfo != null)
            inspectorModeInfo.SetValue(serialize, InspectorMode.Debug, null);
        SerializedProperty localIdProp = serialize.FindProperty("m_LocalIdentfierInFile");
        #if UNITY_5 || UNITY2017_2_OR_NEWER
        idInFile = localIdProp.longValue;
        #else
        idInFile = localIdProp.intValue;
        #endif
        return idInFile;
    }

    public MonoBehaviour FindObjectByLocalIdInFile(long localIdInFile)
    {
        foreach (MonoBehaviour monoBehaviourInstance in FindObjectsOfType<MonoBehaviour>())
        {
            if (GetObjectLocalIdInFile(monoBehaviourInstance) == localIdInFile)
            {
                return monoBehaviourInstance;
            }
        }

        return null;
    }

    // checks if target gameObject is persisted through LoadScene (ie if it owns the IPersistent on one of its components or one of its parents does)
    public bool IsPersistent(GameObject targetObject)
    {
        bool hasReachedTopParent = false;

        while (!hasReachedTopParent)
        {
            MonoBehaviour[] monoInstances = targetObject.GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour monoInstance in monoInstances)
            {
                if (monoInstance is IPersistent)
                {
                    return true;
                }
            }

            if (targetObject.transform.parent == null)
            {
                hasReachedTopParent = true;
            }
            else
            {
                targetObject = targetObject.transform.parent.gameObject;
            }
        }

        return false;
    }
}
