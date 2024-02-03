using UnityEngine;

namespace ProjectFiles.TetrisGrid
{
    public struct GridParameters
    {
        public Vector2Int GridSize;
        public Vector2Int GridSquareSize;
        public InventoryItem[,] GridInventoryData;
    }
}