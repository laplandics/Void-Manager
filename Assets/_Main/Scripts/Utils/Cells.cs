using System.Collections.Generic;
using Content.WorldSpace;
using UnityEngine;
using Grid = Tools.Grid;

namespace Utils
{
    public class Cells
    {
        private readonly Dictionary<Vector2Int, List<Entity>> _cellsMap = new();

        public void OnEntitySpawned(Vector3 position, Entity entity)
        {
            var cell = Grid.WorldToGrid(position);
            
            if (_cellsMap.TryGetValue(cell, out var entities)) { entities.Add(entity); }
            else { _cellsMap.Add(cell, new List<Entity> { entity }); }
        }

        public void OnEntityDespawned(Vector3 position, Entity entity)
        {
            var cell = Grid.WorldToGrid(position);

            if (!_cellsMap.TryGetValue(cell, out var entities)) return;
            entities.Remove(entity);
            if (entities.Count == 0) _cellsMap.Remove(cell);
        }
        
        public void OnEntityChangedCell(Vector3 newPosition, Vector3 oldPosition, Entity entity)
        {
            OnEntityDespawned(oldPosition, entity);
            OnEntitySpawned(newPosition, entity);
        }
        
        public List<Entity> GetEntitiesByPosition(Vector3 position) => _cellsMap[Grid.WorldToGrid(position)];
    }
}