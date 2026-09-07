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

        public void SetColor(Color color)
        { if (!TryGet<EntityComponentColor>(nameof(ColorTint), out var col)) return; col.stream.Value = color; }

        public void SetRenderOrder(int renderOrder)
        { if (!TryGet<EntityComponentInt>(nameof(RenderOrder), out var rend)) return; rend.stream.Value = renderOrder; }

        public void SetBuildingProgress(float progress)
        { if (!TryGet<EntityComponentFloat>(nameof(OnBuild), out var build)) return; build.stream.Value = progress; }
        
        public void AddBuildingProgress(float progress)
        { if (!TryGet<EntityComponentFloat>(nameof(OnBuild), out var build)) return; build.stream.Value += progress; }

        public float GetBuildingProgress()
        { return !TryGet<EntityComponentFloat>(nameof(OnBuild), out var build) ? 0f : build.stream.Value; }

        public bool GetCollisions(out IReadOnlyList<string> collisions)
        {
            collisions = new List<string>();
            if (!TryGet<EntityComponentStringList>(nameof(Collision), out var coll)) return false;
            collisions = coll.stream.Value;
            return true;
        }

        public void SetVisibility(bool visibility)
        { if (!TryGet<EntityComponentBool>(nameof(Visibility), out var vis)) return; vis.stream.Value = visibility; }

        public void AddWorldUI(string uiElementName)
        { if (!TryGet<EntityComponentStringList>(nameof(EntityUI), out var ui)) return; ui.stream.AddUnique(uiElementName); }
    }
}