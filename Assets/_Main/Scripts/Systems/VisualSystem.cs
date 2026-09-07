using System;
using static Configs.ComponentConfig;
using System.Collections.Generic;
using Constants;
using Content.WorldSpace;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EntitySystems
{
    [RequiredTags(nameof(Sprite), nameof(Visibility), nameof(ColorTint), nameof(RenderOrder))]
    public class VisualSystem : EntitySystem
    {
        private readonly Dictionary<string, SpriteRenderer> _renderersMap = new();
        
        public override void RegisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Add(id);
            RegistrationsMap[id] = new List<IDisposable>();
            
            var visualObject = new GameObject(Names.ENTITY_VISUAL_OBJECT_NAME);
            var renderer = visualObject.AddComponent<SpriteRenderer>();
            visualObject.transform.SetParent(entity.entityObject.transform);
            _renderersMap[id] = renderer;
            
            var spriteComponent = entity.GetComponent<EntityComponentString>(nameof(Sprite));
            RegistrationsMap[id].Add(spriteComponent.stream.Subscribe((newValue, _)
                => OnNewSpriteNameSet(entity, newValue)));
            
            var visibilityComponent = entity.GetComponent<EntityComponentBool>(nameof(Visibility));
            RegistrationsMap[id].Add(visibilityComponent.stream.Subscribe((newValue, _)
                => OnVisibilityChanged(entity, newValue)));
            
            var colorTintComponent = entity.GetComponent<EntityComponentColor>(nameof(ColorTint));
            RegistrationsMap[id].Add(colorTintComponent.stream.Subscribe((newValue, _)
                => OnColorTintChanged(entity, newValue)));
            
            var renderOrderComponent = entity.GetComponent<EntityComponentInt>(nameof(RenderOrder));
            RegistrationsMap[id].Add(renderOrderComponent.stream.Subscribe((newValue, _)
                => OnRenderOrderChanged(entity, newValue)));
        }

        private void OnNewSpriteNameSet(Entity entity, string spriteName) =>
            _renderersMap[entity.id].sprite = R.LoadSprite(spriteName);
        

        private void OnVisibilityChanged(Entity entity, bool visible) =>
            _renderersMap[entity.id].enabled = visible;
        

        private void OnColorTintChanged(Entity entity, Color color) =>
            _renderersMap[entity.id].color = color;
        
        private void OnRenderOrderChanged(Entity entity, int renderOrder) =>
            _renderersMap[entity.id].sortingOrder = renderOrder;
        
        public override void UnregisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Remove(id);
            RegistrationsMap[id].ForEach(r => r.Dispose());
            RegistrationsMap.Remove(id);
            
            Object.Destroy(_renderersMap[entity.id].gameObject);
            _renderersMap.Remove(id);
        }
    }
}