using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class HierarchyWindowGroupHeader
{
    static HierarchyWindowGroupHeader()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
    }

    static void HierarchyWindowItemOnGUI(int instanceId, Rect selectionRect)
    {
        var gameObject = EditorUtility.InstanceIDToObject(instanceId) as GameObject;

        var subHeaderStyle = new GUIStyle();
        subHeaderStyle.fontStyle = FontStyle.Bold;
        subHeaderStyle.normal.textColor = Color.white;

        if (gameObject != null)
        {
            if (gameObject.name.StartsWith("---", System.StringComparison.Ordinal))
            {
                EditorGUI.DrawRect(selectionRect, Color.gray);
                EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Replace("-", "").ToUpperInvariant());
            }
            else if (gameObject.name.StartsWith("--", System.StringComparison.Ordinal))
            {
                EditorGUI.DrawRect(selectionRect, new Color(0.6f, 0.6f, 0.6f, 1f));
                selectionRect.x += 50;
                EditorGUI.LabelField(selectionRect, gameObject.name.Replace("-", ""), subHeaderStyle);
            }
        }
    }
}