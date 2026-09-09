using System;
using static Configs.ComponentConfig;
using System.Collections.Generic;
using Content.WorldSpace;
using UnityEngine;

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
        }

        private static void SetPosition(Entity entity, Vector3 value)
        { entity.entityObject.transform.position = value; }

        public override void UnregisterEntity(Entity entity)
        {
            RegisteredEntities.Remove(entity.id);
            RegistrationsMap[entity.id].ForEach(r => r.Dispose());
            RegistrationsMap.Remove(entity.id);
        }
    }
}