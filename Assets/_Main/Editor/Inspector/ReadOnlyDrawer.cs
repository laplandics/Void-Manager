#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom drawer for [ReadOnly] attribute.
/// Renders the field (and all its children) as disabled in the Inspector.
/// Works with: primitives, enums, strings, Vector2/3/4, Color,
/// structs, [Serializable] classes, arrays, List<T>, nested types, etc.
/// </summary>
[CustomPropertyDrawer(typeof(ReadOnlyAttribute), useForChildren: true)]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Delegate height calculation to the default drawer so that
        // foldouts, lists, and nested objects all size correctly.
        return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        bool wasEnabled = GUI.enabled;
        GUI.enabled = false;

        // Draw the full property (including foldout and all children).
        EditorGUI.PropertyField(position, property, label, includeChildren: true);

        GUI.enabled = wasEnabled;
    }
}
#endif
