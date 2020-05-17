using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public static string GenerateUniqueID()
    {
        return Guid.NewGuid().ToString("N");
    }

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

    static public Vector3 MultiplyVector3By(Vector3 targetVector, float by, bool is2D = true)
    {
        return new Vector3(targetVector.x * by, targetVector.y * by, is2D ? targetVector.z : targetVector.z * by);
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

    static public long GetObjectLocalIdInFile(UnityEngine.Object _object)
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

    public static List<MonoBehaviour> GetChildren(MonoBehaviour monoObject)
    {
        List<MonoBehaviour> children = new List<MonoBehaviour>();

        for (int i = 0; i < monoObject.transform.childCount - 1; i++)
        {
            children.Add(monoObject.transform.GetChild(i).GetComponent<MonoBehaviour>());
        }

        return children;
    }

    public static MonoBehaviour GetChildByComponent(MonoBehaviour monoObject, string component)
    {
        for (int i = 0; i < monoObject.transform.childCount ; i++)
        {
            Transform monoChild = monoObject.transform.GetChild(i);

            if (monoChild.GetComponent(component))
            {
                return monoChild.GetComponent<MonoBehaviour>();
            }
        }

        return null;
    }

    public static Vector3 MoveToVelocity(Vector3 move, GameObject targetObject)
    {
        Vector3 distance = move - targetObject.transform.position;
        return distance * (1/Time.deltaTime);
    }
}

