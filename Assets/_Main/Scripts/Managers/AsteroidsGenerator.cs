using System.Collections.Generic;
using Configs;
using Tools;
using UnityEngine;
using Utils;

namespace Managers
{
    public class AsteroidsGenerator
    {
        private const string ASTEROID_SPRITES_PREFIX = "Asteroid";
        private const string ASTEROID_CENTER_ENTITY_TYPE = "Asteroid Center";
        private const string ASTEROID_TILE_ENTITY_TYPE = "Asteroid Tile";
        
        private const int ATLAS_SIZE = 9;
        
        private const float CHUNK_SPAWN_PROBABILITY = 2f;
        private const int SPAWN_BUFFER = 2;
        
        private const float NOISE_SCALE = 1f;
        private const float NOISE_STRENGTH = 0.3f;
        
        private readonly Transform _container;
        
        private readonly int _chunkSpawnSeed;
        private readonly int _startCellSeed;
        private readonly int _asteroidRadiusSeed;
        private readonly int _asteroidSpriteSeed;
        
        private readonly string[] _asteroidSpritesNames;
        
        public AsteroidsGenerator()
        {
            _chunkSpawnSeed = Seed.GetSeed("chunk_spawn_seed");
            _startCellSeed = Seed.GetSeed("start_cell_seed");
            _asteroidRadiusSeed = Seed.GetSeed("asteroid_radius_seed");
            _asteroidSpriteSeed = Seed.GetSeed("asteroid_sprite_seed");
            
            _asteroidSpritesNames = new string[ATLAS_SIZE];
            for (var i = 0; i < ATLAS_SIZE; i++)
            { _asteroidSpritesNames[i] = $"{ASTEROID_SPRITES_PREFIX} {i}"; }

            _container = new GameObject("Asteroids").transform;
        }
        
        public void Launch()
        {
            G.Resolve<TileMap>().OnChunkAdded += OnChunkAdded;
            G.Resolve<TileMap>().OnChunkRemoved += OnChunkRemoved;
        }

        private void OnChunkAdded(Chunk chunk)
        {
            float cX = chunk.Index.x;
            float cY = chunk.Index.y;
            if (!ShouldSpawnAsteroidHere(cX, cY)) return;
            
            var possibleStartCells = new List<Cell>();
            foreach (var (cellIndex, cell) in chunk.CellsMap)
            { if (!CellInSpawnArea(cellIndex)) continue; possibleStartCells.Add(cell); }
            
            var rIndex = Mathf.FloorToInt(PositionHash.Value01(_startCellSeed, cX, cY) * possibleStartCells.Count);
            var startCell = possibleStartCells[rIndex];
            SpawnAsteroid(startCell.CenterPosition, ASTEROID_CENTER_ENTITY_TYPE);
            
            var radius = 1f + Mathf.Round(PositionHash.Value01(_asteroidRadiusSeed, cX, cY) * 3) * 0.5f;
            foreach (var (cellIndex, cell) in chunk.CellsMap)
            {
                if (cell == startCell) continue;
                
                var distance = Vector2Int.Distance(startCell.LocalIndex, cellIndex);

                var x = cell.CenterPosition.x;
                var y = cell.CenterPosition.y;
                var noise = GetNoise(x, y);
                
                if (distance > radius * noise) continue;
                SpawnAsteroid(cell.CenterPosition, ASTEROID_TILE_ENTITY_TYPE);
            }
        }

        private bool ShouldSpawnAsteroidHere(float x, float y)
        { return PositionHash.Value01(_chunkSpawnSeed, x, y) * 100f < CHUNK_SPAWN_PROBABILITY; }

        private bool CellInSpawnArea(Vector2Int cellLocalIndex)
        {
            if (cellLocalIndex.x < SPAWN_BUFFER || cellLocalIndex.x >= TileMap.CHUNK_SIZE - SPAWN_BUFFER ||
                cellLocalIndex.y < SPAWN_BUFFER || cellLocalIndex.y >= TileMap.CHUNK_SIZE - SPAWN_BUFFER)
                return false;
            
            return true;
        }

        private void SpawnAsteroid(Vector3 position, string type)
        {
            var spriteIndex = Mathf.FloorToInt(PositionHash.Value01(_asteroidSpriteSeed,
                position.x, position.y) * _asteroidSpritesNames.Length);
            var spriteName = _asteroidSpritesNames[spriteIndex];
            
            var asteroidData = EntityConfig.Asteroid(type, spriteName, position);
            G.Resolve<Entities>().New(asteroidData, _container);
        }
        
        private float GetNoise(float x, float y)
        {
            var noise = Mathf.PerlinNoise(x * NOISE_SCALE, y * NOISE_SCALE);
            var mult = Mathf.Lerp(1f - NOISE_STRENGTH, 1f + NOISE_STRENGTH, noise);
            return mult;
        }
        
        private void OnChunkRemoved(Chunk chunk)
        {
            foreach (var (_, cell) in chunk.CellsMap)
            {
                foreach (var cellEntity in cell.GetEntities())
                {
                    if (cellEntity?.type is ASTEROID_CENTER_ENTITY_TYPE or ASTEROID_TILE_ENTITY_TYPE)
                    { G.Resolve<Entities>().Delete(cellEntity.id); }
                }
            }
        }
    }
}