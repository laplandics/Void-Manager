using System;
using System.Collections.Generic;
using System.Globalization;
using Constants;
using Content.WorldSpace;
using UnityEngine;

namespace Tools
{
    public static class EntityComponentDataConverter
    {
        public static EntityComponent FromEntry(string dataEntry)
        {
            var split = dataEntry.Split(Separators.ENTITY_COMPONENT_DATA_SEPARATOR);
            var type = split[0];
            var tag = split[1];
            var value = split[2];
            
            EntityComponent data = type switch
            {
                nameof(EntityComponentEmpty) => new EntityComponentEmpty
                    { tag = tag },
                
                nameof(EntityComponentInt) => new EntityComponentInt
                    { tag = tag, stream = new Reactive<int>(int.Parse(value)) },
                
                nameof(EntityComponentFloat) => new EntityComponentFloat
                    { tag = tag, stream = new Reactive<float>(float.Parse(value, CultureInfo.InvariantCulture)) },
                
                nameof(EntityComponentBool) => new EntityComponentBool
                    { tag = tag, stream = new Reactive<bool>(bool.Parse(value)) },
                
                nameof(EntityComponentString) => new EntityComponentString
                    { tag = tag, stream = new Reactive<string>(value) },
                
                nameof(EntityComponentVector2) => new EntityComponentVector2
                    { tag = tag, stream = new Reactive<Vector2>(EntityComponentVectorValueParser.GetVector2(value)) },
                
                nameof(EntityComponentVector3) => new EntityComponentVector3
                    { tag = tag, stream = new Reactive<Vector3>(EntityComponentVectorValueParser.GetVector3(value)) },
                
                nameof(EntityComponentColor) => new EntityComponentColor
                    { tag = tag, stream = new Reactive<Color>(EntityComponentColorValueParser.GetColor(value)) },
                
                nameof(EntityComponentStringList) => new EntityComponentStringList
                    { tag = tag, stream = new ReactiveList<string>(EntityComponentListValueParser.GetList(value)) },
                
                _ => throw new Exception("Failed to parse entity component data entry")
            };
            
            return data;
        }
        
        public static string ToEntry(EntityComponent baseData)
        {
            var entry = baseData switch
            {
                EntityComponentEmpty emptyData => 
                    MakeString(nameof(EntityComponentEmpty), emptyData.tag),
                
                EntityComponentInt intData =>
                    MakeString(nameof(EntityComponentInt), intData.tag, intData.stream.Value),
                
                EntityComponentFloat floatData =>
                    MakeString(nameof(EntityComponentFloat), floatData.tag, floatData.stream.Value),
                
                EntityComponentBool boolData =>
                    MakeString(nameof(EntityComponentBool), boolData.tag, boolData.stream.Value),
                
                EntityComponentString stringData =>
                    MakeString(nameof(EntityComponentString), stringData.tag, stringData.stream.Value),
                
                EntityComponentVector2 vector2Data =>
                    MakeString(nameof(EntityComponentVector2), vector2Data.tag, vector2Data.stream.Value),
                
                EntityComponentVector3 vector3Data =>
                    MakeString(nameof(EntityComponentVector3), vector3Data.tag, vector3Data.stream.Value),
                
                EntityComponentColor colorData => 
                    MakeString(nameof(EntityComponentColor), colorData.tag, colorData.stream.Value),
                
                EntityComponentStringList stringArrayData =>
                    MakeString(nameof(EntityComponentStringList), stringArrayData.tag, stringArrayData.stream.Value),
                    
                _ => string.Empty
            };
            
            return entry;
        }

        private static string MakeString(string type, string tag)
        {
            return $"{type}{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
                   $"{tag}{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
                   string.Empty;
        }
        
        private static string MakeString(string type, string tag, int value)
        {
            return $"{type}{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
                   $"{tag}{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
                   $"{value.ToString()}";
        }
        
        private static string MakeString(string type, string tag, float value)
        {
            return $"{type}{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
                   $"{tag}{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
                   $"{value.ToString(CultureInfo.InvariantCulture)}";
        }
        
        private static string MakeString(string type, string tag, bool value)
        {
            return $"{type}{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
                   $"{tag}{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
                   $"{value.ToString()}";
        }
        
        private static string MakeString(string type, string tag, string value)
        {
            return $"{type}{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
                   $"{tag}{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
                   $"{value}";
        }
        
        private static string MakeString(string type, string tag, Vector2 value)
        {
            return $"{type}{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
                   $"{tag}{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
                   EntityComponentVectorValueParser.GetString(value);
        }
        
        private static string MakeString(string type, string tag, Vector3 value)
        {
            return $"{type}{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
                   $"{tag}{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
                   EntityComponentVectorValueParser.GetString(value);
        }
        
        private static string MakeString(string type, string tag, Color value)
        {
            return $"{type}{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
                   $"{tag}{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
                   EntityComponentColorValueParser.GetString(value);
        }
        
        private static string MakeString(string type, string tag, IEnumerable<string> value)
        {
            return $"{type}{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
                   $"{tag}{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
                   EntityComponentListValueParser.GetString(value);
        }
    }
}