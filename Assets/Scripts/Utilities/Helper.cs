using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class Helper : MonoBehaviour {

    static public bool IsV2Equal(Vector2 a, Vector2 b)
    {
        return Vector2.SqrMagnitude(a - b) < 0.0001;
    }

    static public bool IsV3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.0001;
    }

    static public Vector2 CopyVector2(Vector2 a)
    {
        return new Vector2(a.x, a.y);
    }

    static public Vector3 CopyVector3(Vector3 a)
    {
        return new Vector3(a.x, a.y, a.y);
    }

    static public string UcFirst(string theString)
    {
        if (string.IsNullOrEmpty(theString))
        {
            return string.Empty;
        }

        char[] theChars = theString.ToCharArray();
        theChars[0] = char.ToUpper(theChars[0]);

        return new string(theChars);
    }

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
}

