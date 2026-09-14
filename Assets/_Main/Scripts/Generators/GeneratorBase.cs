using System;
using System.Collections;
using System.Collections.Generic;
using Content.WorldSpace;
using Data;
using Tools;
using UnityEngine;
using Utils;

namespace Generators
{
    public class GeneratorBase
    {
        private readonly int _buffer;
        private readonly Transform _container;
        
        private readonly int _positionSeed;
        private readonly int _rotationSeed;
        private readonly int _spriteSeed;
        private readonly float _spawnProbability;
        private readonly Func<EntityData> _dataFactory;
        private readonly Action<Entity> _onEntityGeneratedCallback;

        private readonly string[] _spriteNames;

        private Vector2Int _cachedGridOrigin;
        private bool _hasCachedGridOrigin;
        
        private readonly Dictionary<Vector2Int, EntityData> _generatedEntitiesMap = new();
        private readonly Vector3[] _rotationAngles = { new(0f, 0f, 0f), new(0f, 0f, 90f),
            new(0f, 0f, 180f), new(0f, 0f, 270f) };
        
        private static Camera Cam => G.Resolve<GameCamera>().GetCamera();
        
        public GeneratorBase(int buffer, Transform container, string[] spriteNames, int positionSeed,
        int rotationSeed, int spriteSeed, float spawnProbability, Func<EntityData> dataFactory,
        Action<Entity> onEntityGeneratedCallback = null)
        {
            _buffer = buffer;
            _container = container;
            _spriteNames = spriteNames;
            _positionSeed = positionSeed;
            _rotationSeed = rotationSeed;
            _spriteSeed = spriteSeed;
            _spawnProbability = spawnProbability;
            _dataFactory = dataFactory;
            _onEntityGeneratedCallback = onEntityGeneratedCallback;
        }

        public IEnumerator GenerationRoutine()
        {
            var camTransform = Cam.transform;
            
            while (true)
            {
                var cameraPosition = camTransform.position;
                var gridOrigin = new Vector2Int(
                    Mathf.FloorToInt(cameraPosition.x),
                    Mathf.FloorToInt(cameraPosition.y));

                if (_hasCachedGridOrigin && gridOrigin == _cachedGridOrigin)
                { yield return null; continue; }

                _cachedGridOrigin = gridOrigin;
                _hasCachedGridOrigin = true;

                var halfHeight = Cam.orthographicSize;
                var halfWidth = Cam.orthographicSize * Cam.aspect;

                var heightWithBuffer = halfHeight + _buffer;
                var widthWithBuffer = halfWidth + _buffer;

                var yMin = Mathf.FloorToInt(cameraPosition.y - heightWithBuffer);
                var yMax = Mathf.CeilToInt(cameraPosition.y + heightWithBuffer);
                
                var xMin = Mathf.FloorToInt(cameraPosition.x - widthWithBuffer);
                var xMax = Mathf.CeilToInt(cameraPosition.x + widthWithBuffer);

                var entitiesToDelete = new List<Vector2Int>(_generatedEntitiesMap.Keys);

                for (var y = yMin; y < yMax; y++)
                {
                    for (var x = xMin; x < xMax; x++)
                    {
                        var position = new Vector2Int(x, y);
                        if (_generatedEntitiesMap.ContainsKey(position))
                        { entitiesToDelete.Remove(position); continue; }
                        
                        var shouldSpawnObject = PositionHash.Value01(_positionSeed, x, y)
                            * 100f < _spawnProbability;
                        if (!shouldSpawnObject) continue;

                        var spriteIndex = Mathf.FloorToInt(
                            PositionHash.Value01(_spriteSeed, x, y) * _spriteNames.Length);
                        var spriteName = _spriteNames[spriteIndex];
                        
                        var rotationIndex = Mathf.FloorToInt(
                            PositionHash.Value01(_rotationSeed, x, y) * _rotationAngles.Length);
                        var rotation = _rotationAngles[rotationIndex];
                        
                        var entityData = _dataFactory.Invoke();
                        var entity = G.Resolve<Entities>().New(entityData);
                        
                        entity.Actions.SetPosition(new Vector3(position.x, position.y, 0f));
                        entity.Actions.SetSprite(spriteName);
                        entity.Actions.SetRotation(rotation);
                        
                        entity.entityObject.transform.SetParent(_container.transform);
                        
                        _generatedEntitiesMap.Add(position, entityData);
                        _onEntityGeneratedCallback?.Invoke(entity);
                    }
                }

                foreach (var entityPosition in entitiesToDelete)
                {
                    if (!_generatedEntitiesMap.TryGetValue(entityPosition, out var starData)) continue;
                    G.Resolve<Entities>().Delete(starData.id);
                    _generatedEntitiesMap.Remove(entityPosition);
                }
                
                yield return null;
            }
        }
    }
}