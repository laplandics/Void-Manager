using System.Collections.Generic;
using Constants;
using Content.WorldSpace;
using UnityEngine;

namespace Configs
{
    public static class ComponentConfig
    {
        public static string MaxHp(float value) =>
            $"{nameof(EntityComponentFloat)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{nameof(MaxHp)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{value}";
        
        public static string CurrentHp(float value) =>
            $"{nameof(EntityComponentFloat)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{nameof(CurrentHp)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{value}";
        
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
            $"{Tools.EntityComponentColorValueParser.GetString(value)}";

        public static string RenderOrder(int value) =>
            $"{nameof(EntityComponentInt)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{nameof(RenderOrder)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{value}";
        
        public static string Visibility(bool value) =>
            $"{nameof(EntityComponentBool)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{nameof(Visibility)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{value}";
        
        public static string OnBuild(int value) =>
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
            $"{Tools.EntityComponentListValueParser.GetString(value)}";
        
        public static string Position(Vector3 value) =>
            $"{nameof(EntityComponentVector3)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{nameof(Position)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{Tools.EntityComponentVectorValueParser.GetString(value)}";
        
        public static string FollowCursor() =>
            $"{nameof(EntityComponentEmpty)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{nameof(FollowCursor)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}";
        
        public static string EntityUI(List<string> value) =>
            $"{nameof(EntityComponentStringList)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{nameof(EntityUI)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{Tools.EntityComponentListValueParser.GetString(value)}";
        
        public static string Collision(List<string> value) =>
            $"{nameof(EntityComponentStringList)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{nameof(Collision)}" +
            $"{Separators.ENTITY_COMPONENT_DATA_SEPARATOR}" +
            $"{Tools.EntityComponentListValueParser.GetString(value)}";
    }
}