using System;
using System.Collections.Generic;
using Content.WorldSpace;
using UnityEngine;
using static Configs.ComponentConfig;
using Grid = Tools.Grid;

namespace EntitySystems
{
    [RequiredTags(nameof(CameraFollow), nameof(Position))]
    public class CameraFollowSystem : EntitySystem
    {
        public override void RegisterEntity(Entity entity)
        {
            var id = entity.id;
            RegistrationsMap[id] = new List<IDisposable>();
            
            if (RegisteredEntities.Count != 0)
            { throw new Exception("Only one entity can be registered in CameraFollowSystem at a time."); }
            RegisteredEntities.Add(id);

            var positionComponent = entity.GetComponent<EntityComponentVector3>(nameof(Position));
            RegistrationsMap[id].Add(positionComponent.stream.Subscribe((newValue, _) => MoveCamera(newValue)));
        }

        private static void MoveCamera(Vector3 position)
        {
            var camera = G.Resolve<GameCamera>().GetCamera();
            
            if (camera == null) return;
            var cameraObject = camera.gameObject;
            var currentPosition = cameraObject.transform.position;
            
            var gridPosition = Grid.Snap(position);
            cameraObject.transform.position = new Vector3(gridPosition.x, gridPosition.y, currentPosition.z);
        }
        
        public override void UnregisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Remove(id);
            RegistrationsMap[id].ForEach(r => r.Dispose());
            RegistrationsMap.Remove(id);
        }
    }
}