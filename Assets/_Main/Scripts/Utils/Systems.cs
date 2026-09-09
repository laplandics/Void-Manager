using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EntitySystems;

namespace Utils
{
    public class Systems
    {
        private readonly List<SystemRegistration> _systemRegistrations = new();
        private readonly Dictionary<Type, EntitySystem> _systemsMap = new();
        
        public void Initialize()
        {
            AddSystemRegistration(new VisualSystem());
            AddSystemRegistration(new OnBuildSystem());
            AddSystemRegistration(new PositionSystem());
            AddSystemRegistration(new FollowCursorSystem());
            AddSystemRegistration(new EntityUISystem());
            AddSystemRegistration(new CollisionSystem());
            AddSystemRegistration(new OnGridSystem());
        }

        private void AddSystemRegistration(EntitySystem system)
        {
            var systemType = system.GetType();
            var requiredTagsAttr = systemType.GetCustomAttribute<RequiredTagsAttribute>();
            var requiredTags = new HashSet<string>();
            if (requiredTagsAttr != null)
            { requiredTags = new HashSet<string>(requiredTagsAttr.Tags); }
            var forbiddenTagsAttr = systemType.GetCustomAttribute<ForbiddenTagsAttribute>();
            var forbiddenTags = new HashSet<string>();
            if (forbiddenTagsAttr != null)
            { forbiddenTags = new HashSet<string>(forbiddenTagsAttr.Tags); }
            _systemRegistrations.Add(new SystemRegistration
            { System = system, RequiredTags = requiredTags, ForbiddenTags = forbiddenTags, });
            
            _systemsMap.Add(systemType, system);
        }

        public void Update(string entityId)
        {
            var entity = G.Resolve<Entities>().GetEntity(entityId);
            if (entity == null) return;
            var entityComponents = entity.components;
            var componentsTags = entityComponents.Select(c => c.tag).ToHashSet();
            foreach (var systemRegistration in _systemRegistrations)
            {
                var requiredTagsIncluded = systemRegistration.RequiredTags.All(componentsTags.Contains);
                var forbiddenTagAbsents = !systemRegistration.ForbiddenTags.Any(componentsTags.Contains);

                if (systemRegistration.System.RegisteredEntities.Contains(entityId))
                {
                    if (!forbiddenTagAbsents || !requiredTagsIncluded)
                    { systemRegistration.System.UnregisterEntity(entity); }
                }
                else
                {
                    if (forbiddenTagAbsents && requiredTagsIncluded)
                    { systemRegistration.System.RegisterEntity(entity); }
                }
            }
        }

        public T GetSystem<T>() where T : EntitySystem
        { if (!_systemsMap.TryGetValue(typeof(T), out var system)) return null; return (T)system; }
        
        private class SystemRegistration
        {
            public EntitySystem System;
            public HashSet<string> RequiredTags;
            public HashSet<string> ForbiddenTags;
        }
    }
}