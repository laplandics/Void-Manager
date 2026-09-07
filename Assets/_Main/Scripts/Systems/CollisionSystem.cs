using System.Collections.Generic;
using Content.WorldSpace;
using UnityEngine;
using Object = UnityEngine.Object;
using static Configs.ComponentConfig;

namespace EntitySystems
{
    [RequiredTags(nameof(Collision))]
    public class CollisionSystem : EntitySystem
    {
        private readonly Dictionary<string, CollisionDetector> _detectorsMap = new();
        
        public override void RegisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Add(id);
            
            var entityType = entity.type;
            var colliderName = $"{entityType}Collider";
            var colliderPrefab = R.LoadColliderObject(colliderName);
            var colliderObject = Object.Instantiate(colliderPrefab, entity.entityObject.transform, false);
            colliderObject.name = "Collider";
            var detector = colliderObject.AddComponent<CollisionDetector>();
            
            var collisionComponent = entity.GetComponent<EntityComponentStringList>(nameof(Collision));

            _detectorsMap[id] = detector;
            
            detector.Activate(collisionComponent);
        }
        
        public override void UnregisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Remove(id);

            if (!_detectorsMap.TryGetValue(id, out var detector)) return;
            Object.Destroy(detector.gameObject);
            _detectorsMap.Remove(id);
        }
    }

    [RequireComponent(typeof(Collider2D))]
    public class CollisionDetector : MonoBehaviour
    {
        private EntityComponentStringList _collisionComponent;
        
        public void Activate(EntityComponentStringList collisionComponent)
        { _collisionComponent = collisionComponent; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var otherEntity = other.GetComponentInParent<EntityObject>();
            if (otherEntity == null) return;
            var entityID = otherEntity.entity.id;
            _collisionComponent.stream.AddUnique(entityID);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var otherEntity = other.GetComponentInParent<EntityObject>();
            if (otherEntity == null) return;
            var entityID = otherEntity.entity.id;
            _collisionComponent.stream.Remove(entityID);
        }
    }
}