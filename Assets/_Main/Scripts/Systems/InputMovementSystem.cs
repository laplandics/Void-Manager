using System;
using System.Collections;
using Content.WorldSpace;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using Grid = Tools.Grid;
using static Configs.ComponentConfig;

namespace EntitySystems
{
    [RequiredTags(nameof(InputMovement), nameof(Position))]
    public class InputMovementSystem : EntitySystem
    {
        private const float ENTITY_INPUT_MOVEMENT_DELAY_BEFORE_MOVE_CONSTANTLY = 0.5f;
        private const float ENTITY_INPUT_MOVEMENT_DELAY_BEFORE_NEXT_STEP = 0.1f;
        
        private Entity _controlledEntity;
        private InputAction _activeAction;
        private Vector2 _direction;
        
        public override void RegisterEntity(Entity entity)
        {
            var id = entity.id;
            _controlledEntity = entity;
            
            if (RegisteredEntities.Count != 0)
            { throw new Exception("Only one entity can be registered in InputMovementSystem at a time."); }
            RegisteredEntities.Add(id);
            
            G.Resolve<Coroutines>().Start(MoveRoutine(), _controlledEntity.entityObject);
        }

        private IEnumerator MoveRoutine()
        {
            var moveUp = G.Resolve<Inputs>().PlayerMoveUp;
            var moveDown = G.Resolve<Inputs>().PlayerMoveDown;
            var moveLeft = G.Resolve<Inputs>().PlayerMoveLeft;
            var moveRight = G.Resolve<Inputs>().PlayerMoveRight;

            var actionPerformed = false;
                
            while (true)
            {
                if (_activeAction != null)
                {
                    if (_activeAction.IsPressed() && !actionPerformed)
                    {
                        var x = _direction.x * Grid.GRID_CELL_SIZE;
                        var y = _direction.y * Grid.GRID_CELL_SIZE;
                        var newDirection = new Vector3(x, y, 0);
                        var currentPos = _controlledEntity.Actions.GetPosition();
                        var newPosition = currentPos + newDirection;
                        _controlledEntity.Actions.SetPosition(newPosition);

                        actionPerformed = true;
                        yield return null;
                        continue;
                    }

                    var timeToStartMoving = 0f;
                    while (_activeAction.IsPressed() && actionPerformed)
                    {
                        if (timeToStartMoving < ENTITY_INPUT_MOVEMENT_DELAY_BEFORE_MOVE_CONSTANTLY)
                        { timeToStartMoving += Time.deltaTime; yield return null; continue; }
                        
                        var x = _direction.x * Grid.GRID_CELL_SIZE;
                        var y = _direction.y * Grid.GRID_CELL_SIZE;
                        var newDirection = new Vector3(x, y, 0);
                        var currentPos = _controlledEntity.Actions.GetPosition();
                        var newPosition = currentPos + newDirection;
                        _controlledEntity.Actions.SetPosition(newPosition);
                        
                        var timeToRepeatStep = 0f;
                        while (timeToRepeatStep < ENTITY_INPUT_MOVEMENT_DELAY_BEFORE_NEXT_STEP && _activeAction.IsPressed())
                        { timeToRepeatStep += Time.deltaTime; yield return null; }
                        
                        yield return null;
                    }
                    
                    _activeAction = null;
                }
                
                if (moveUp.WasPressedThisFrame())
                { _activeAction = moveUp; _direction = Vector2.up; actionPerformed = false; }
                
                else if (moveDown.WasPressedThisFrame())
                { _activeAction = moveDown; _direction = Vector2.down; actionPerformed = false; }
                
                else if (moveLeft.WasPressedThisFrame())
                { _activeAction = moveLeft; _direction = Vector2.left; actionPerformed = false; }
                
                else if (moveRight.WasPressedThisFrame())
                { _activeAction = moveRight; _direction = Vector2.right; actionPerformed = false; }
                
                yield return null;
            }
        }
        
        public override void UnregisterEntity(Entity entity)
        {
            var id = entity.id;
            RegisteredEntities.Remove(id);
            _controlledEntity = null;
            RegistrationsMap[id].ForEach(r => r.Dispose());
            RegistrationsMap.Remove(id);
        }
    }
}