using System.Globalization;
using Constants;
using UnityEngine;

namespace Tools
{
    public static class EntityComponentColorValueParser
    {
        public static string GetString(Color value)
        {
            return $"{value.r.ToString(CultureInfo.InvariantCulture)}" +
                   $"{Separators.ENTITY_COMPONENT_COLOR_VALUE_SEPARATOR}" +
                   $"{value.g.ToString(CultureInfo.InvariantCulture)}" +
                   $"{Separators.ENTITY_COMPONENT_COLOR_VALUE_SEPARATOR}" +
                   $"{value.b.ToString(CultureInfo.InvariantCulture)}" +
                   $"{Separators.ENTITY_COMPONENT_COLOR_VALUE_SEPARATOR}" +
                   $"{value.a.ToString(CultureInfo.InvariantCulture)}";
        }

        public static Color GetColor(string value)
        {
            var parts = value.Split(Separators.ENTITY_COMPONENT_COLOR_VALUE_SEPARATOR);
            return new Color(
                float.Parse(parts[0], CultureInfo.InvariantCulture),
                float.Parse(parts[1], CultureInfo.InvariantCulture),
                float.Parse(parts[2], CultureInfo.InvariantCulture),
                float.Parse(parts[3], CultureInfo.InvariantCulture));
        }
    }
}