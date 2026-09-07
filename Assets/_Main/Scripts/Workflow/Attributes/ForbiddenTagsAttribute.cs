using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class ForbiddenTagsAttribute : PropertyAttribute
{
    public string[] Tags { get; }
    public ForbiddenTagsAttribute(params string[] tags) => Tags = tags;
}