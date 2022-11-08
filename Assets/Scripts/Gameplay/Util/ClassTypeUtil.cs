using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility method connected with class type with unity script components
/// </summary>
public static class ClassTypeUtil
{
    /// <summary>
    /// Gets list of scipts on GameObject that inherit given class or interface type.
    /// This only should be used to find scipts that inherit or implements some particular class or interface.
    /// Use GetComponent for other cases.
    /// </summary>
    /// <typeparam name="T">Class or interface type that should be looked for (inlcuding inherited or implemented classes)</typeparam>
    /// <param name="gameObject">Object where scripts should be looked up</param>
    /// <returns>list of scipts all matching, or empty list if none found</returns>
    public static List<T> GetScriptWithClassTypeFromGameobject<T>(GameObject gameObject)
    {
        List<T> interfaces = new();

        foreach (MonoBehaviour script in gameObject.GetComponents<MonoBehaviour>())
        {
            if (script is T interfaceType) interfaces.Add(interfaceType);
        }

        return interfaces;
    }
}
