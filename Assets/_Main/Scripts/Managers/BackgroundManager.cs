using System.Collections.Generic;
using Configs;
using Data;
using Tools;
using UnityEngine;
using Utils;

namespace Managers
{
    public class BackgroundManager
    {
        private const int BUFFER = 2;
        private const float SPAWN_PROBABILITY = 1f;
        
        private readonly int _positionSeed = Seed.GetSeed("star_position_seed");
        private readonly int _colorSeed = Seed.GetSeed("star_color_seed");
        
        private readonly Transform _container = new GameObject("Background").transform;
        private readonly Dictionary<Vector2Int, EntityData> _generatedStarsMap = new();

        private readonly Color[] _starsColors = { Color.white, Color.crimson, Color.yellow, Color.cyan };
        
        private static Camera Cam => G.Resolve<GameCamera>().GetCamera();
        
        public void Launch()
        {
            G.Resolve<GameCamera>().OnCameraMoved += OnCameraMoved;
            OnCameraMoved(G.Resolve<GameCamera>().GetCameraPosition());
        }

        private void OnCameraMoved(Vector3 cameraPosition)
        {
            var halfHeight = Cam.orthographicSize;
            var halfWidth = Cam.orthographicSize * Cam.aspect;

            var heightWithBuffer = halfHeight + BUFFER;
            var widthWithBuffer = halfWidth + BUFFER;

            var yMin = Mathf.FloorToInt(cameraPosition.y - heightWithBuffer);
            var yMax = Mathf.CeilToInt(cameraPosition.y + heightWithBuffer);
            
            var xMin = Mathf.FloorToInt(cameraPosition.x - widthWithBuffer);
            var xMax = Mathf.CeilToInt(cameraPosition.x + widthWithBuffer);

            var starsToDelete = new List<Vector2Int>(_generatedStarsMap.Keys);

            for (var y = yMin; y < yMax; y++)
            {
                for (var x = xMin; x < xMax; x++)
                {
                    var position = new Vector2Int(x, y);
                    if (_generatedStarsMap.ContainsKey(position))
                    { starsToDelete.Remove(position); continue; }
                    
                    var shouldSpawnObject = PositionHash.Value01(_positionSeed, x + 1000, y + 1000) * 100f < SPAWN_PROBABILITY;
                    if (!shouldSpawnObject) continue;
                    
                    var colorIndex = Mathf.FloorToInt(PositionHash.Value01(_colorSeed, x, y) * _starsColors.Length);
                    var color = _starsColors[colorIndex];
                    
                    var starData = EntityConfig.Star(new Vector3(position.x, position.y, 0f), color);
                    G.Resolve<Entities>().New(starData, _container);
                    
                    _generatedStarsMap.Add(position, starData);
                }
            }

            foreach (var starPosition in starsToDelete)
            {
                if (!_generatedStarsMap.TryGetValue(starPosition, out var starData)) continue;
                G.Resolve<Entities>().Delete(starData.id);
                _generatedStarsMap.Remove(starPosition);
            }
        }
    }
}