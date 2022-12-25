using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility class that helps with detecting class of type / interface of scripts on Unity GameObjects
/// </summary>
public static class ClassTypeUtil
{
    /// <summary>
    /// Gets list of scipts on GameObject that inherit given class or implement interface type.
    /// This should be used to find scipts on GameObject that inherit or implements some particular class or interface.
    /// Use GetComponent for other cases.
    /// </summary>
    /// <typeparam name="T">Class or interface type that should be looked for and type of list that will be returned</typeparam>
    /// <param name="gameObject">GameObject where scripts should be searched</param>
    /// <returns>List of all matching scripts, or empty list if none found</returns>
    public static List<T> GetScriptWithClassTypeFromGameobject<T>(GameObject gameObject)
    {
        List<T> classesOrInterfaces = new();

        foreach (MonoBehaviour script in gameObject.GetComponents<MonoBehaviour>())
        {
            if (script is T classOrInterface) classesOrInterfaces.Add(classOrInterface);
        }

        return classesOrInterfaces;
    }
}
