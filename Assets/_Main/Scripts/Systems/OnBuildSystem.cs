using System;
using System.Collections;
using Content.WorldSpace;
using System.Collections.Generic;
using System.Linq;
using Constants;
using UIBinders;
using UnityEngine;
using Utils;
using static Configs.ComponentConfig;

namespace EntitySystems
{
    [RequiredTags(nameof(OnBuild), nameof(Sprite))]
    public class OnBuildSystem : EntitySystem
    {
        public const int ENTITY_BUILD_STAGE_0_PROGRESS_MAX = 9;
        
        private readonly Dictionary<string, int> _stageMap = new();
        private readonly Dictionary<string, Color> _cachedColorsMap = new();
        
        public override void RegisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Add(id);
            RegistrationsMap[id] = new List<IDisposable>();
            
            var otherComponentsRaw = entity.ClearComponents();
            if (otherComponentsRaw is { Count: > 0 })
            {
                var otherComponents = otherComponentsRaw.Select(c =>
                    c.Replace(Separators.ENTITY_COMPONENT_DATA_SEPARATOR,
                        Separators.ENTITY_CACHED_COMPONENTS_DATA_SEPARATOR)).ToList();
                entity.AddComponent(CachedComponents(otherComponents), silent: true);
            }
            
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
            }
        }

        private static int CalculateStage(float progress)
        { return progress switch { < ENTITY_BUILD_STAGE_0_PROGRESS_MAX => 0, _ => 1 }; }

        private IEnumerator Stage0Routine(Entity entity)
        {
            yield return null;
            entity.Actions.AddWorldUI(nameof(EntityUIBuildProgressBar));
            
            _cachedColorsMap[entity.id] = entity.Actions.GetColor();
            entity.Actions.SetColor(Color.grey);

            var wait = new WaitFramePausable();
            while (entity.Actions.GetBuildingProgress() <= ENTITY_BUILD_STAGE_0_PROGRESS_MAX)
            { yield return wait; entity.Actions.AddBuildingProgress(Time.deltaTime); }
        }

        private IEnumerator Stage1Routine(Entity entity)
        {
            var cachedComponentsComponent = entity.GetComponent<EntityComponentStringList>(nameof(CachedComponents));
            if (cachedComponentsComponent != null)
            {
                foreach (var savedComponentCachedEntry in cachedComponentsComponent.stream.Value)
                {
                    var savedComponentEntry = savedComponentCachedEntry.Replace
                    (
                        Separators.ENTITY_CACHED_COMPONENTS_DATA_SEPARATOR,
                        Separators.ENTITY_COMPONENT_DATA_SEPARATOR
                    );

                    entity.AddComponent(savedComponentEntry, silent: true);
                }
            }
            
            entity.Actions.RemoveWorldUI(nameof(EntityUIBuildProgressBar));
            entity.Actions.SetColor(_cachedColorsMap[entity.id]);
            _cachedColorsMap.Remove(entity.id);
            
            entity.RemoveComponent(nameof(CachedComponents), silent: true);
            
            yield return null;
            entity.RemoveComponent(nameof(OnBuild));
        }
        
        public override void UnregisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Remove(id);
            RegistrationsMap[id].ForEach(r => r.Dispose());
            RegistrationsMap.Remove(id);
            _stageMap.Remove(id);
            _cachedColorsMap.Remove(id);
        }
    }
}