using Constants;
using UnityEngine;

namespace Tools
{
    public static class Grid
    {
        public static Vector3 Snap(Vector3 pos)
        {
            var cellIndex = WorldToGrid(pos);
            var cellCenter = GridToWorldCenter(cellIndex);
            return cellCenter;
        }
        
        private static Vector2Int WorldToGrid(Vector3 worldPos)
        {
            const float cellSize = Values.GRID_CELL_SIZE;
            var x = Mathf.FloorToInt(worldPos.x / cellSize);
            var y = Mathf.FloorToInt(worldPos.y / cellSize);

            return new Vector2Int(x, y);
        }
        
        private static Vector3 GridToWorldCenter(Vector2Int gridPos)
        {
            const float cellSize = Values.GRID_CELL_SIZE;
            const float half = cellSize / 2f;

            var x = gridPos.x * cellSize + half;
            var y = gridPos.y * cellSize + half;

            return new Vector3(x, y, 0);
        }
    }
}