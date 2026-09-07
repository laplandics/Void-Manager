using System;
using System.Collections.Generic;
using Content.WorldSpace;
using Helpers;
using UnityEngine;
using Utils;
using static Configs.ComponentConfig;

namespace EntitySystems
{
    [RequiredTags(nameof(EntityUI))]
    public class EntityUISystem : EntitySystem
    {
        private readonly Dictionary<string, WorldUIInfo> _entitiesUIsMap = new();
        
        public override void RegisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Add(id);
            RegistrationsMap[id] = new List<IDisposable>();
            
            var entityWorldUIInfo = new WorldUIInfo(
                "EntityUI",
                Guid.NewGuid().ToString(),
                R.EntityWorldUIRootAsset,
                entity.entityObject.transform);
            
            RegistrationsMap[id].Add(entityWorldUIInfo.IsReady.SubscribeSilently((_, _)
                => OnEntityUIReady(entity, entityWorldUIInfo)));
            
            _entitiesUIsMap[id] = entityWorldUIInfo;
            G.Resolve<UI>().SetWorldUI(entityWorldUIInfo);
        }

        private void OnEntityUIReady(Entity entity, WorldUIInfo uiInfo)
        {
            if (!uiInfo.IsReady.Value) return;
            uiInfo.Renderer.worldSpaceSize = new Vector2(320f, 320f);
            
            var uiComponent = entity.GetComponent<EntityComponentStringList>(nameof(EntityUI));

            foreach (var existingUi in uiComponent.stream.Value) OnEntityUIAdd(uiInfo, existingUi);
            
            RegistrationsMap[entity.id].Add(uiComponent.stream.OnAdd(uiElementName
                => OnEntityUIAdd(uiInfo, uiElementName)));
            RegistrationsMap[entity.id].Add(uiComponent.stream.OnRemove(uiElementName
                => OnEntityUIRemove(uiInfo, uiElementName)));
        }

        private void OnEntityUIAdd(WorldUIInfo uiInfo, string uiElementName)
        {
            var instance = Tools.EntityWorldUITypesRegistry.CreateInstance(uiElementName, uiInfo);
            
            instance.Add(out var uiElementId);
            uiInfo.AttachedElementsNamesIdsMap[uiElementName] = uiElementId;
        }

        private void OnEntityUIRemove(WorldUIInfo uiInfo, string uiElementName)
        {
            if (!uiInfo.AttachedElementsNamesIdsMap.TryGetValue(uiElementName, out var uiElementId)) return;
            if (!G.Resolve<UI>().TryGetUIElement(uiElementId, out var uiElement)) return;
            
            uiElement.Remove();
            uiInfo.AttachedElementsNamesIdsMap.Remove(uiElementName);
        }
        
        public override void UnregisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Remove(id);
            RegistrationsMap[id].ForEach(r => r.Dispose());
            RegistrationsMap.Remove(id);
            G.Resolve<UI>().RemoveWorldUI(_entitiesUIsMap[id]);
            _entitiesUIsMap.Remove(id);
        }
    }
}