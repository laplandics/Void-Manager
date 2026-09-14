using System.Collections.Generic;
using Content.WorldSpace;
using UnityEngine;
using static Configs.ComponentConfig;

namespace Helpers
{
    public class EntityActions
    {
        private Entity Owner { get; }
        public EntityActions(Entity owner) { Owner = owner; }

        private bool TryGet<T>(string tag, out T component) where T : EntityComponent
        { component = Owner.GetComponent<T>(tag); return component != null; }
        
        public void SetPosition(Vector3 position)
        { if (!TryGet<EntityComponentVector3>(nameof(Position), out var pos)) return; pos.stream.Value = position; }

        public void SetRotation(Vector3 angles)
        { if (!TryGet<EntityComponentVector3>(nameof(Rotation), out var rot)) return; rot.stream.Value = angles; }
        
        public Vector3 GetPosition()
        { return !TryGet<EntityComponentVector3>(nameof(Position), out var pos) ? Vector3.zero : pos.stream.Value; }

        public void SetBuildingProgress(float progress)
        { if (!TryGet<EntityComponentFloat>(nameof(OnBuild), out var build)) return; build.stream.Value = progress; }
        
        public void AddBuildingProgress(float progress)
        { if (!TryGet<EntityComponentFloat>(nameof(OnBuild), out var build)) return; build.stream.Value += progress; }

        public float GetBuildingProgress()
        { return !TryGet<EntityComponentFloat>(nameof(OnBuild), out var build) ? 0f : build.stream.Value; }

        public void SetSprite(string spriteName)
        { if (!TryGet<EntityComponentString>(nameof(Sprite), out var spr)) return; spr.stream.Value = spriteName; }
        
        public void AddWorldUI(string uiElementName)
        { if (!TryGet<EntityComponentStringList>(nameof(EntityUI), out var ui)) return; ui.stream.AddUnique(uiElementName); }

        public void RemoveWorldUI(string uiElementName)
        { if (!TryGet<EntityComponentStringList>(nameof(EntityUI), out var ui)) return; ui.stream.Remove(uiElementName); }
        
        public void SetColor(Color color)
        { if (!TryGet<EntityComponentColor>(nameof(ColorTint), out var col)) return; col.stream.Value = color; }

        public Color GetColor()
        { return !TryGet<EntityComponentColor>(nameof(ColorTint), out var col) ? Color.violetRed : col.stream.Value; }
    }
}