using System;
using System.Collections.Generic;
using static Configs.ComponentConfig;
using Content.WorldSpace;

namespace EntitySystems
{
    [RequiredTags(nameof(MaxHp), nameof(CurrentHp))]
    public class HealthSystem : EntitySystem
    {
        public override void RegisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Add(id);
            RegistrationsMap[id] = new List<IDisposable>();
            
            var maxHpComponent = entity.GetComponent<EntityComponentFloat>(nameof(MaxHp));
            var currentHpComponent = entity.GetComponent<EntityComponentFloat>(nameof(CurrentHp));
            
            RegistrationsMap[id].Add(maxHpComponent.stream.Subscribe((newValue, oldValue)
                => OnMaxHpChanged(id, newValue, oldValue)));
            
            RegistrationsMap[id].Add(currentHpComponent.stream.Subscribe((newValue, oldValue)
                => OnCurrentHpChanged(id, newValue, oldValue)));
        }

        private static void OnMaxHpChanged(string entityId, float newValue, float oldValue)
        {
            
        }

        private static void OnCurrentHpChanged(string entityId, float newValue, float oldValue)
        {
            
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