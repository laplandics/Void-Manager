using static Configs.ComponentConfig;
using System.Collections;
using System.Linq;
using Content.WorldSpace;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;

namespace EntitySystems
{
    [RequiredTags(nameof(FollowCursor))]
    public class FollowCursorSystem : EntitySystem
    {
        public FollowCursorSystem()
        { G.Resolve<Coroutines>().Start(FollowRoutine()); }
        
        public override void RegisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Add(id);
            entity.Actions.SetRenderOrder(10);
        }

        private IEnumerator FollowRoutine()
        {
            var cam = Camera.main;
            var mouse = Mouse.current;
            
            while (true)
            {
                if (cam == null) { cam = Camera.main; yield return null; continue; }
                if (mouse == null) { mouse = Mouse.current; yield return null; continue; }

                for (var i = 0; i < RegisteredEntities.Count; i++)
                {
                    var entityId = RegisteredEntities.ElementAt(i);
                    var entity = G.Resolve<Entities>().GetEntity(entityId);
                    if (!string.IsNullOrEmpty(entityId))
                    {
                        var mouseScreenPos = mouse.position.ReadValue();
                        var worldPos = cam.ScreenToWorldPoint(mouseScreenPos);
                        var world2DPos = new Vector3(worldPos.x, worldPos.y, 0);
                        entity.Actions.SetPosition(world2DPos);
                    }
                    
                    yield return null;
                }
                
                yield return null;
            }
        }
        
        public override void UnregisterEntity(Entity entity)
        {
            RegisteredEntities.Remove(entity.id);
            entity.Actions.SetRenderOrder(0);
        }
    }
}