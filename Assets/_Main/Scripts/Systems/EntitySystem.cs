using System;
using System.Collections.Generic;
using Content.WorldSpace;

namespace EntitySystems
{
    public abstract class EntitySystem
    {
        public readonly HashSet<string> RegisteredEntities = new();
        protected readonly Dictionary<string, List<IDisposable>> RegistrationsMap = new();
        
        public abstract void RegisterEntity(Entity entity);
        public abstract void UnregisterEntity(Entity entity);
    }
}