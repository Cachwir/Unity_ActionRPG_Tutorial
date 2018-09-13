using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class ClassReflector
{
    public static bool HasProperty(object concernedObject, string propertyName)
    {
        return concernedObject.GetType().GetProperty(propertyName) != null;
    }

    public static bool GetBoolProperty(object concernedObject, string propertyName)
    {
        return (bool) concernedObject.GetType().GetProperty(propertyName).GetValue(concernedObject, null);
    }

    public static bool HasMethod(object concernedObject, string methodName)
    {
        return concernedObject.GetType().GetMethod(methodName) != null;
    }

    public static void DynamicInvoke(object concernedObject, string methodName, object parameter1 = null)
    {
        GetMethodInfo(concernedObject, methodName).Invoke(concernedObject, PrepareMethodParameters(concernedObject, methodName, parameter1));
    }

    public static bool DynamicInvokeBool(object concernedObject, string methodName, object parameter1 = null)
    {
        return (bool) GetMethodInfo(concernedObject, methodName).Invoke(concernedObject, PrepareMethodParameters(concernedObject, methodName, parameter1));
    }

    public static MethodInfo GetMethodInfo(object concernedObject, string methodName)
    {
        return concernedObject.GetType().GetMethod(methodName);
    }

    public static object[] PrepareMethodParameters(object concernedObject, string methodName, object parameter1 = null)
    {
        ParameterInfo[] parametersList = GetMethodInfo(concernedObject, methodName).GetParameters();

        object[] parameters = new object[0];

        if (parametersList.Length == 1)
        {
            if (parameter1 == null)
            {
                throw new System.Exception("The method " + methodName + " in the object " + concernedObject.GetType().FullName + " requires 1 parameter and you gave none.");
            }
            parameters = new object[1] { parameter1 };
        }
        else if (parametersList.Length > 1)
        {
            throw new System.Exception("The method " + methodName + " in the object " + concernedObject.GetType().FullName + " requires at least 2 parameters, which is not currently implemented.");
        }

        return parameters;
    }
}
