using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StringDropdownAttribute))]
public class StringDropdownDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.String)
        { EditorGUI.LabelField(position, label.text, "Используйте StringDropdown только со string"); return; }
        
        var dropdownAttribute = (StringDropdownAttribute)attribute;
        var methodName = dropdownAttribute.MethodName;
        var targetObject = property.serializedObject.targetObject;
        var targetType = targetObject.GetType();
        var method = targetType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic |
                                                      BindingFlags.Static | BindingFlags.Instance);
        if (method == null)
        {
            EditorGUI.LabelField(position, label.text, $"Метод '{dropdownAttribute.MethodName}' не найден");
            return;
        }
        var result = method.Invoke(method.IsStatic ? null : targetObject, null);
        if (result is not string[] options || options.Length == 0)
        {
            EditorGUI.LabelField(position, label.text, $"Метод '{dropdownAttribute.MethodName}' пуст");
            return;
        }
        
        var currentIndex = System.Array.IndexOf(options, property.stringValue);
        if (currentIndex < 0) currentIndex = 0;

        var selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, options);
        property.stringValue = options[selectedIndex];
    }
}