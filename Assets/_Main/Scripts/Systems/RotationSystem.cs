using System;
using System.Collections.Generic;
using Content.WorldSpace;
using UnityEngine;
using static Configs.ComponentConfig;

namespace EntitySystems
{
    [RequiredTags(nameof(Rotation))]
    public class RotationSystem : EntitySystem
    {
        public override void RegisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Add(id);
            RegistrationsMap[id] = new List<IDisposable>();
            
            var rotationComponent = entity.GetComponent<EntityComponentVector3>(nameof(Rotation));
            RegistrationsMap[id].Add(rotationComponent.stream.Subscribe((newValue, _)
                => OnRotationChanged(entity, newValue)));
        }

        private void OnRotationChanged(Entity entity, Vector3 angles)
        { entity.entityObject.transform.eulerAngles = angles; }
        
        public override void UnregisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Remove(id);
            RegistrationsMap[id].ForEach(r => r.Dispose());
            RegistrationsMap.Remove(id);
        }
    }
}