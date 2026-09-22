using System;
using System.Collections.Generic;
using System.Linq;
using Content.WorldSpace;
using UnityEngine;
using Grid = Tools.Grid;

namespace Utils
{
    public class TileMap
    {
        public const int CHUNK_SIZE = 8;
        public event Action<Chunk> OnChunkAdded;
        public event Action<Chunk> OnChunkRemoved;
        
        private readonly Dictionary<Vector2Int, Chunk> _chunksMap = new();
        public IReadOnlyDictionary<Vector2Int, Chunk> ChunksMap => _chunksMap;
        
        public void AddChunk(Vector2Int chunkIndex)
        {
            if (_chunksMap.ContainsKey(chunkIndex)) return;
            
            const int area = CHUNK_SIZE * CHUNK_SIZE;
            var cells = new Dictionary<Vector2Int, Cell>(area);
            
            var i = 0;
            for (var y = 0; y < CHUNK_SIZE; y++)
            {
                for (var x = 0; x < CHUNK_SIZE; x++)
                {
                    var cellLocalIndex = new Vector2Int(x, y);
                    
                    var cellGlobalX = chunkIndex.x * CHUNK_SIZE + x;
                    var cellGlobalY = chunkIndex.y * CHUNK_SIZE + y;
                    var cellGlobalIndex = new Vector2Int(cellGlobalX, cellGlobalY);
                    
                    var cellCenterPosition = Grid.GridToWorldCenter(cellGlobalIndex);
                    cells[cellLocalIndex] = new Cell(cellCenterPosition, cellLocalIndex, cellGlobalIndex);
                    
                    i++;
                }
            }

            var chunk = new Chunk(chunkIndex, cells);
            _chunksMap.Add(chunkIndex, chunk);
            
            OnChunkAdded?.Invoke(chunk);
        }

        public Vector2Int GetChunkIndex(Vector3 worldPosition)
        {
            var x = Mathf.FloorToInt(worldPosition.x / CHUNK_SIZE);
            var y = Mathf.FloorToInt(worldPosition.y / CHUNK_SIZE);
            return new Vector2Int(x, y);
        }

        public Chunk GetChunk(Vector3 worldPosition) => _chunksMap[GetChunkIndex(worldPosition)];

        public void RemoveChunk(Vector2Int chunkIndex)
        {
            if (!_chunksMap.TryGetValue(chunkIndex, out var chunk)) return;
            OnChunkRemoved?.Invoke(chunk);
            _chunksMap.Remove(chunkIndex);
        }
        
        public Cell GetCell(Vector3 worldPosition)
        {
            var chunk = GetChunk(worldPosition);
            var cellGlobalIndex = Grid.WorldToGrid(worldPosition);
            
            var localX = (int)Mathf.Repeat(cellGlobalIndex.x, CHUNK_SIZE);
            var localY = (int)Mathf.Repeat(cellGlobalIndex.y, CHUNK_SIZE);
            var localIndex = new Vector2Int(localX, localY);
            
            var cell = chunk.CellsMap[localIndex];
            return cell;
        }
    }

    public class Chunk
    {
        public Vector2Int Index;
        public readonly Dictionary<Vector2Int, Cell> CellsMap;

        public Chunk(Vector2Int index, Dictionary<Vector2Int, Cell> cells)
        { Index = index; CellsMap = cells; }
    }

    public class Cell
    {
        public Vector3 CenterPosition;
        public Vector2Int LocalIndex;
        public Vector2Int GlobalIndex;
        private readonly HashSet<Entity> _entities = new();
        
        public Entity[] GetEntities() => _entities.ToArray();
        
        public Cell(Vector3 centerPosition, Vector2Int localIndex, Vector2Int globalIndex)
        { CenterPosition = centerPosition; LocalIndex = localIndex; GlobalIndex = globalIndex; }

        public void AddEntity(Entity entity) { _entities.Add(entity); }
        public void RemoveEntity(Entity entity) { _entities.Remove(entity); }
    }
}