using System;
using static Configs.ComponentConfig;
using System.Collections.Generic;
using Content.WorldSpace;
using UnityEngine;
using Utils;

namespace EntitySystems
{
    [RequiredTags(nameof(Position))]
    public class PositionSystem : EntitySystem
    {
        public override void RegisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Add(id);
            RegistrationsMap[id] = new List<IDisposable>();
            
            var component = entity.GetComponent<EntityComponentVector3>(nameof(Position));
            
            RegistrationsMap[id].Add(component.stream.Subscribe((newValue, oldValue)
                => SetPosition(entity, newValue, oldValue)));
        }

        private static void SetPosition(Entity entity, Vector3 newValue, Vector3 oldValue)
        {
            var gridPos = Tools.Grid.Snap(newValue);
            entity.entityObject.transform.localPosition = gridPos;
            
            var previousCell = G.Resolve<TileMap>().GetCell(oldValue);
            previousCell.RemoveEntity(entity);
            
            var newCell = G.Resolve<TileMap>().GetCell(newValue);
            newCell.AddEntity(entity);
        }
        
        public override void UnregisterEntity(Entity entity)
        {
            RegisteredEntities.Remove(entity.id);
            RegistrationsMap[entity.id].ForEach(r => r.Dispose());
            RegistrationsMap.Remove(entity.id);
        }
    }
}