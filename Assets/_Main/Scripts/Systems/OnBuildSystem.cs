using System;
using System.Collections;
using Content.WorldSpace;
using System.Collections.Generic;
using System.Linq;
using Constants;
using Content.UISpace;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using static Configs.ComponentConfig;

namespace EntitySystems
{
    [RequiredTags(nameof(OnBuild), nameof(Sprite))]
    public class OnBuildSystem : EntitySystem
    {
        private readonly Dictionary<string, int> _stageMap = new();

        private readonly Color _inactiveColor = new(0.5f, 0.5f, 0.5f, 0.5f);
        private readonly Color _buildingColor = new(0.6f, 0.6f, 0.6f, 0.9f);
        
        public override void RegisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Add(id);
            RegistrationsMap[id] = new List<IDisposable>();
            
            var otherComponentsRaw = entity.ClearComponents(
                nameof(OnBuild),
                nameof(Sprite),
                nameof(Position),
                nameof(Visibility),
                nameof(CachedComponents),
                nameof(EntityUI),
                nameof(ColorTint),
                nameof(Collision),
                nameof(RenderOrder));
            
            var otherComponents = otherComponentsRaw.Select(c =>
                c.Replace(Separators.ENTITY_COMPONENT_DATA_SEPARATOR,
                    Separators.ENTITY_CACHED_COMPONENTS_DATA_SEPARATOR)).ToList();
            
            entity.AddComponent(CachedComponents(otherComponents), silent: true);
            
            var onBuildComponent = entity.GetComponent<EntityComponentFloat>(nameof(OnBuild));
            RegistrationsMap[id].Add(onBuildComponent.stream.Subscribe((newValue, _)
                => ChangeBuildStage(entity, newValue)));
        }
        
        private void ChangeBuildStage(Entity entity, float progress)
        {
            var stage = CalculateStage(progress);

            if (_stageMap.TryGetValue(entity.id, out var currentStage) && currentStage == stage) return;
            _stageMap[entity.id] = stage;
            
            switch (stage)
            {
                case 0: G.Resolve<Coroutines>().Start(Stage0Routine(entity), entity.entityObject); break;
                case 1: G.Resolve<Coroutines>().Start(Stage1Routine(entity), entity.entityObject); break;
                case 2: G.Resolve<Coroutines>().Start(Stage2Routine(entity), entity.entityObject); break;
            }
        }

        private IEnumerator Stage0Routine(Entity entity)
        {
            _stageMap[entity.id] = 0;
            
            yield return null;
            entity.Actions.SetColor(_inactiveColor);
            entity.AddComponent(FollowCursor());
            
            yield return null;
            entity.Actions.SetVisibility(true);
            
            while(true)
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (entity.Actions.GetCollisions(out var collisions) && collisions.Count > 0)
                    { yield return null; continue; }
                    
                    entity.Actions.SetColor(_buildingColor);
                    entity.Actions.SetBuildingProgress(0.1f);
                    yield break;
                }

                if (Mouse.current.rightButton.wasPressedThisFrame)
                { entity.Actions.SetBuildingProgress(float.MaxValue); G.Resolve<Entities>().Delete(entity.id); }
                
                yield return null;
            }
        }

        private IEnumerator Stage1Routine(Entity entity)
        {
            _stageMap[entity.id] = 1;
            
            yield return null;
            entity.RemoveComponent(nameof(FollowCursor));
            entity.Actions.AddWorldUI(nameof(EntityWorldUIOnBuildProgressBar));
            
            const int maxStageProgress = Values.ENTITY_BUILD_STAGE_1_PROGRESS_MAX;
            var wait = new WaitForSeconds(0.1f);
            
            while (entity.Actions.GetBuildingProgress() <= maxStageProgress)
            { yield return wait; entity.Actions.AddBuildingProgress(0.1f); }
        }

        private IEnumerator Stage2Routine(Entity entity)
        {
            _stageMap[entity.id] = 2;
            
            var cachedComponentsComponent = entity.GetComponent<EntityComponentStringList>(nameof(CachedComponents));
            if (cachedComponentsComponent == null) yield break;
            
            foreach (var savedComponentCachedEntry in cachedComponentsComponent.stream.Value)
            {
                var savedComponentEntry = savedComponentCachedEntry.Replace
                (
                    Separators.ENTITY_CACHED_COMPONENTS_DATA_SEPARATOR,
                    Separators.ENTITY_COMPONENT_DATA_SEPARATOR
                );
                
                entity.AddComponent(savedComponentEntry, silent: true);
            }
            
            entity.GetComponent<EntityComponentStringList>(nameof(EntityUI))
                .stream.Remove(nameof(EntityWorldUIOnBuildProgressBar));

            entity.GetComponent<EntityComponentColor>(nameof(ColorTint))
                .stream.Value = Color.white;
            
            yield return null;
            entity.RemoveComponent(nameof(CachedComponents), silent: true);
            entity.RemoveComponent(nameof(OnBuild));
        }

        public static int CalculateStage(float progress)
        { return progress switch { <= 0 => 0, > 0 and <= Values.ENTITY_BUILD_STAGE_1_PROGRESS_MAX => 1, _ => 2 }; }
        
        public override void UnregisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Remove(id);
            RegistrationsMap[id].ForEach(r => r.Dispose());
            RegistrationsMap.Remove(id);
            _stageMap.Remove(id);
        }
    }
}