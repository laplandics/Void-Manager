using System;
using System.Collections;
using System.Collections.Generic;
using Content.WorldSpace;
using UnityEngine;
using Utils;
using static Configs.ComponentConfig;

namespace EntitySystems
{
    [RequiredTags(nameof(OnGrid), nameof(Position))]
    public class OnGridSystem : EntitySystem
    {
        public override void RegisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Add(id);
            RegistrationsMap[id] = new List<IDisposable>();
            
            var positionComponent = entity.GetComponent<EntityComponentVector3>(nameof(Position));
            RegistrationsMap[id].Add(positionComponent.stream.Subscribe((newValue, _)
                => G.Resolve<Coroutines>().Start(SnapToGridDelayed(entity, newValue), entity.entityObject)));
        }

        private IEnumerator SnapToGridDelayed(Entity entity, Vector3 position)
        {
            yield return null;
            var gridPos = Tools.Grid.Snap(position);
            entity.Actions.SetPosition(gridPos);
        }

        public override void UnregisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Remove(id);
            RegistrationsMap[id].ForEach(r => r.Dispose());
            RegistrationsMap.Remove(id);
        }
    }
}