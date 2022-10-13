using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Commonly used functions, properties, and helpers
/// </summary>
public static class Utils
{
    // Math constants
    public const float pi = Mathf.PI;
    public const float sqrt3 = 1.7320508075688772935274463415058723669428052538103806280558069794f;

    // Common functions
    // Mostly these are just so I don't have to look up the same thing over and over
    /// <summary>
    /// Converts the given Vector3 from local space to world space
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static Vector3 LocalToWorld(Transform transform, Vector3 vec)
    {
        return transform.TransformPoint(vec);
    }
    /// <summary>
    /// Converts the given Vector3 from world space to local space
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static Vector3 WorldToLocal(Transform transform, Vector3 vec)
    {
        return transform.InverseTransformPoint(vec);
    }
}