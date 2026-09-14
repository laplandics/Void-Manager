using System;
using static Configs.ComponentConfig;
using System.Collections.Generic;
using Content.WorldSpace;
using UnityEngine;
using Utils;

namespace EntitySystems
{
    [RequiredTags(nameof(Position))]
    public class PositionSystem : EntitySystem
    {
        public override void RegisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Add(id);
            RegistrationsMap[id] = new List<IDisposable>();
            
            var component = entity.GetComponent<EntityComponentVector3>(nameof(Position));
            
            RegistrationsMap[id].Add(component.stream.Subscribe((newValue, _)
                => SetPosition(entity, newValue)));
            
            RegistrationsMap[id].Add(component.stream.SubscribeSilently((newValue, oldValue)
                => G.Resolve<Cells>().OnEntityChangedCell(newValue, oldValue, entity)));
        }

        private static void SetPosition(Entity entity, Vector3 value)
        {
            var gridPos = Tools.Grid.Snap(value);
            entity.entityObject.transform.position = gridPos;
        }
        
        public override void UnregisterEntity(Entity entity)
        {
            RegisteredEntities.Remove(entity.id);
            RegistrationsMap[entity.id].ForEach(r => r.Dispose());
            RegistrationsMap.Remove(entity.id);
        }
    }
}