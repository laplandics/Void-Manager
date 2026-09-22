using System.Collections.Generic;
using Content.WorldSpace;
using Data;
using UnityEngine;

namespace Configs
{
    public static class ComponentConfig
    {
        public static ComponentData Sprite(string value) => new()
        {
            type = nameof(EntityComponentString),
            tag = nameof(Sprite),
            value = value
        };
        
        public static ComponentData ColorTint(Color value) => new()
        {
            type = nameof(EntityComponentColor),
            tag = nameof(ColorTint),
            value = Converters.EntityComponentColorValueParser.GetString(value)
        };

        public static ComponentData RenderOrder(int value) => new()
        {
            type = nameof(EntityComponentInt),
            tag = nameof(RenderOrder),
            value = value.ToString()
        };

        public static ComponentData Position(Vector3 value) => new()
        {
            type = nameof(EntityComponentVector3),
            tag = nameof(Position),
            value = Converters.EntityComponentVectorValueParser.GetString(value)
        };

        public static ComponentData Rotation(Vector3 value) => new()
        {
            type = nameof(EntityComponentVector3),
            tag = nameof(Rotation),
            value = Converters.EntityComponentVectorValueParser.GetString(value)
        };

        public static ComponentData EntityUI(List<string> value) => new()
        {
            type = nameof(EntityComponentStringList),
            tag = nameof(EntityUI),
            value = Converters.EntityComponentListValueParser.GetString(value)
        };

        public static ComponentData InputMovement() => new()
        {
            type = nameof(EntityComponentEmpty),
            tag = nameof(InputMovement)
        };

        public static ComponentData CameraFollow() => new()
        {
            type = nameof(EntityComponentEmpty),
            tag = nameof(CameraFollow)
        };
    }
}