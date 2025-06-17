using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

public static class UnityExtensions
{
    /// <summary>
    /// Extension method to check if a layer is in a layermask
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    /// <summary>
    /// Extension method to remap a float value from one range to another.
    /// </summary>
    /// <param name="value">Value to remap</param>
    /// <param name="from1">Minimum value of the original range</param>
    /// <param name="to1">Maximum value of the original range</param>
    /// <param name="from2">Minimum value of the new range</param>
    /// <param name="to2">Maximum value of the new range</param>
    /// <returns></returns>
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    /// <summary>
    /// Extension method to delete all the children of a Transform.
    /// </summary>
    /// <param name="transform"></param>
    public static void ClearChildren(this Transform transform)
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Object.DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Extension method to delete all the children of a Transform.
    /// </summary>
    /// <param name="transform">Parent transform to delete children from.</param>
    /// <param name="index">Child index offset to start the deletion from (inclusive).</param>
    public static void ClearChildrenFrom(this Transform transform, int index = 0)
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= index; i--)
        {
            Object.DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
    
    /// <summary>
    /// Adds a component to the destination GameObject from the original T component.
    /// </summary>
    /// <param name="destination">The GameObject to copy the component on.</param>
    /// <param name="original">The component to copy</param>
    /// <typeparam name="T">The type of the component to copy.</typeparam>
    /// <returns>The added component.</returns>
    public static T AddComponentFrom<T>(this GameObject destination, T original) where T : Component
    {
        Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        FieldInfo[] fields = type.GetFields();
        foreach (FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }

        return copy as T;
    }
}