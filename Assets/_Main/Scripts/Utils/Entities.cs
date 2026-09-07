using System;
using System.Collections.Generic;
using Content.WorldSpace;
using Data;

namespace Utils
{
    public class Entities
    {
        private readonly Dictionary<string, Entity> _entitiesMap = new();

        public Entity New(EntityData data)
        {
            data.id = Guid.NewGuid().ToString();
            
            var eIndex = _entitiesMap.Count;
            var entity = new Entity();
            
            _entitiesMap[data.id] = entity;
            entity.OnNew(data, eIndex);
            
            return entity;
        }

        public EntityData Delete(string id)
        {
            var entity = _entitiesMap[id];
            var data = entity.OnDelete();
            _entitiesMap.Remove(id);
            
            return data;
        }
        
        public Entity GetEntity(string id) => _entitiesMap[id];

        public List<Entity> GetEntities(string type)
        {
            var entitiesToReturn = new List<Entity>();
            foreach (var (_, entity) in _entitiesMap)
            {
                if (entity.type != type) continue;
                entitiesToReturn.Add(entity);
            }
            return entitiesToReturn;
        }
    }
}