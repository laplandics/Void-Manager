using System.Globalization;
using Constants;
using UnityEngine;

namespace Tools
{
    public static class EntityComponentVectorValueParser
    {
        public static string GetString(Vector2 value)
        {
            return $"{value.x.ToString(CultureInfo.InvariantCulture)}" +
                   $"{Separators.ENTITY_COMPONENT_VECTOR_VALUE_SEPARATOR}" +
                   $"{value.y.ToString(CultureInfo.InvariantCulture)}";
        }
        
        public static string GetString(Vector3 value)
        {
            return $"{value.x.ToString(CultureInfo.InvariantCulture)}" +
                   $"{Separators.ENTITY_COMPONENT_VECTOR_VALUE_SEPARATOR}" +
                   $"{value.y.ToString(CultureInfo.InvariantCulture)}" +
                   $"{Separators.ENTITY_COMPONENT_VECTOR_VALUE_SEPARATOR}" +
                   $"{value.z.ToString(CultureInfo.InvariantCulture)}";
        }
        
        public static Vector2 GetVector2(string str)
        {
            var parts = str.Split(Separators.ENTITY_COMPONENT_VECTOR_VALUE_SEPARATOR);
            return new Vector2(
                float.Parse(parts[0], CultureInfo.InvariantCulture),
                float.Parse(parts[1], CultureInfo.InvariantCulture));
        }

        public static Vector3 GetVector3(string str)
        {
            var parts = str.Split(Separators.ENTITY_COMPONENT_VECTOR_VALUE_SEPARATOR);
            return new Vector3(
                float.Parse(parts[0], CultureInfo.InvariantCulture),
                float.Parse(parts[1], CultureInfo.InvariantCulture),
                float.Parse(parts[2], CultureInfo.InvariantCulture));
        }
    }
}