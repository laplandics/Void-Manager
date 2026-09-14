using System.Collections.Generic;
using Constants;
using Content.WorldSpace;
using UnityEngine;

namespace Configs
{
    public static class ComponentConfig
    {
        public static string Sprite(string value) =>
            $"{nameof(EntityComponentString)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{nameof(Sprite)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{value}";
        
        public static string ColorTint(Color value) =>
            $"{nameof(EntityComponentColor)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{nameof(ColorTint)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{Converters.EntityComponentColorValueParser.GetString(value)}";

        public static string RenderOrder(int value) =>
            $"{nameof(EntityComponentInt)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{nameof(RenderOrder)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{value}";
        
        public static string OnBuild(int value = 0) =>
            $"{nameof(EntityComponentFloat)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{nameof(OnBuild)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{value}";
        
        public static string CachedComponents(List<string> value) =>
            $"{nameof(EntityComponentStringList)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{nameof(CachedComponents)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{Converters.EntityComponentListValueParser.GetString(value)}";
        
        public static string Position(Vector3 value) =>
            $"{nameof(EntityComponentVector3)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{nameof(Position)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{Converters.EntityComponentVectorValueParser.GetString(value)}";
        
        public static string Rotation(Vector3 value) =>
            $"{nameof(EntityComponentVector3)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{nameof(Rotation)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{Converters.EntityComponentVectorValueParser.GetString(value)}";
        
        public static string EntityUI(List<string> value) =>
            $"{nameof(EntityComponentStringList)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{nameof(EntityUI)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{Converters.EntityComponentListValueParser.GetString(value)}";
        
        public static string InputMovement() =>
            $"{nameof(EntityComponentEmpty)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{nameof(InputMovement)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}";
        
        public static string CameraFollow() =>
            $"{nameof(EntityComponentEmpty)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{nameof(CameraFollow)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}";
    }
}