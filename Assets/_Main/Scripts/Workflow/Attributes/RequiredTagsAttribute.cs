using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class RequiredTagsAttribute : PropertyAttribute
{
    public string[] Tags { get; }
    public RequiredTagsAttribute(params string[] tags) => Tags = tags;
}