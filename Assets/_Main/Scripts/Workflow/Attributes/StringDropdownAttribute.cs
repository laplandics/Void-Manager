using UnityEngine;

public class StringDropdownAttribute : PropertyAttribute
{
    public readonly string MethodName;
    public StringDropdownAttribute(string methodName)
    {
        MethodName = methodName;
    }
}