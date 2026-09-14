using System.Collections.Generic;
using Content.WorldSpace;
using Data;
using UnityEngine;

namespace Utils
{
    public class Entities
    {
        private readonly Dictionary<string, Entity> _entitiesMap = new();
        
        public Entity New(EntityData data, Transform parent = null)
        {
            var entity = new Entity();
            
            _entitiesMap[data.id] = entity;
            entity.OnNew(data, parent);

            var position = entity.entityObject.transform.position;
            G.Resolve<Cells>().OnEntitySpawned(position, entity);
            return entity;
        }

        public EntityData Delete(string id)
        {
            if (!_entitiesMap.TryGetValue(id, out var entity)) return default;
            var position = entity.entityObject.transform.position;
            G.Resolve<Cells>().OnEntityDespawned(position, entity);
            
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
        
        public List<Entity> GetEntities()
        {
            var entitiesToReturn = new List<Entity>();
            foreach (var (_, entity) in _entitiesMap)
            { entitiesToReturn.Add(entity); }
            return entitiesToReturn;
        }
    }
}