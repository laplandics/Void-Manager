using UnityEngine;

namespace Tools
{
    public static class Grid
    {
        public const float GRID_CELL_SIZE = 1.0f;
        
        public static Vector3 Snap(Vector3 pos)
        {
            var cellIndex = WorldToGrid(pos);
            var cellCenter = GridToWorldCenter(cellIndex);
            return cellCenter;
        }

        public static Vector2Int WorldToGrid(Vector3 worldPos)
        {
            var x = Mathf.FloorToInt(worldPos.x / GRID_CELL_SIZE);
            var y = Mathf.FloorToInt(worldPos.y / GRID_CELL_SIZE);

            return new Vector2Int(x, y);
        }
        
        public static Vector3 GridToWorldCenter(Vector2Int gridPos)
        {
            const float half = GRID_CELL_SIZE / 2f;

            var x = gridPos.x * GRID_CELL_SIZE + half;
            var y = gridPos.y * GRID_CELL_SIZE + half;

            return new Vector3(x, y, 0);
        }
    }
}