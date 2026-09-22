using System;
using System.Globalization;
using Content.WorldSpace;
using Data;
using UnityEngine;

namespace Converters
{
    public static class EntityComponentDataConverter
    {
        public static EntityComponent FromData(ComponentData data)
        {
            EntityComponent component = data.type switch
            {
                nameof(EntityComponentEmpty) => new EntityComponentEmpty
                { tag = data.tag },
                
                nameof(EntityComponentInt) => new EntityComponentInt
                { tag = data.tag, stream = new Reactive<int>(int.Parse(data.value)) },
                
                nameof(EntityComponentFloat) => new EntityComponentFloat
                { tag = data.tag, stream = new Reactive<float>(float.Parse(data.value, CultureInfo.InvariantCulture)) },
                
                nameof(EntityComponentBool) => new EntityComponentBool
                { tag = data.tag, stream = new Reactive<bool>(bool.Parse(data.value)) },
                
                nameof(EntityComponentString) => new EntityComponentString
                { tag = data.tag, stream = new Reactive<string>(data.value) },
                
                nameof(EntityComponentVector2) => new EntityComponentVector2
                { tag = data.tag, stream = new Reactive<Vector2>(EntityComponentVectorValueParser.GetVector2(data.value)) },
                
                nameof(EntityComponentVector3) => new EntityComponentVector3
                { tag = data.tag, stream = new Reactive<Vector3>(EntityComponentVectorValueParser.GetVector3(data.value)) },
                
                nameof(EntityComponentColor) => new EntityComponentColor
                { tag = data.tag, stream = new Reactive<Color>(EntityComponentColorValueParser.GetColor(data.value)) },
                
                nameof(EntityComponentStringList) => new EntityComponentStringList
                { tag = data.tag, stream = new ReactiveList<string>(EntityComponentListValueParser.GetList(data.value)) },
                
                _ => throw new Exception("Failed to parse entity component data entry")
            };
            
            return component;
        }
        
        public static ComponentData ToData(EntityComponent component)
        {
            var data = component switch
            {
                EntityComponentEmpty emptyData => MakeData(nameof(EntityComponentEmpty), emptyData.tag, string.Empty),
                
                EntityComponentInt intData => MakeData(nameof(EntityComponentInt), intData.tag,
                    intData.stream.Value.ToString()),
                
                EntityComponentFloat floatData => MakeData(nameof(EntityComponentFloat), floatData.tag,
                    floatData.stream.Value.ToString(CultureInfo.InvariantCulture)),
                
                EntityComponentBool boolData => MakeData(nameof(EntityComponentBool), boolData.tag,
                    boolData.stream.Value.ToString()),
                
                EntityComponentString stringData => MakeData(nameof(EntityComponentString), stringData.tag,
                    stringData.stream.Value),
                
                EntityComponentVector2 vector2Data => MakeData(nameof(EntityComponentVector2), vector2Data.tag,
                    EntityComponentVectorValueParser.GetString(vector2Data.stream.Value)),
                
                EntityComponentVector3 vector3Data => MakeData(nameof(EntityComponentVector3), vector3Data.tag,
                    EntityComponentVectorValueParser.GetString(vector3Data.stream.Value)),
                
                EntityComponentColor colorData => MakeData(nameof(EntityComponentColor), colorData.tag,
                    EntityComponentColorValueParser.GetString(colorData.stream.Value)),
                
                EntityComponentStringList stringArrayData => MakeData(nameof(EntityComponentStringList),
                    stringArrayData.tag, EntityComponentListValueParser.GetString(stringArrayData.stream.Value)),
                    
                _ => new ComponentData()
            };
            
            return data;
        }

        private static ComponentData MakeData(string type, string tag, string value) => new()
            { type = type, tag = tag, value = value };
    }
}