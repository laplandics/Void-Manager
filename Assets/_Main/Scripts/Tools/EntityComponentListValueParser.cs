using System.Collections.Generic;
using System.Linq;
using Constants;

namespace Tools
{
    public static class EntityComponentListValueParser
    {
        public static string GetString(List<string> list)
        {
            if (list == null || list.Count == 0) return null;
            return string.Join(Separators.ENTITY_COMPONENT_ARRAY_VALUE_SEPARATOR, list);
        }
        
        public static string GetString(IEnumerable<string> enumerable)
        {
            var list = enumerable == null ? new List<string>() : enumerable.ToList();
            
            return list.Count == 0 ? string.Empty : 
                string.Join(Separators.ENTITY_COMPONENT_ARRAY_VALUE_SEPARATOR, list);
        }

        public static List<string> GetList(string value)
        {
            return string.IsNullOrEmpty(value) ? new List<string>() :
                value.Split(Separators.ENTITY_COMPONENT_ARRAY_VALUE_SEPARATOR).ToList();
        }
    }
}