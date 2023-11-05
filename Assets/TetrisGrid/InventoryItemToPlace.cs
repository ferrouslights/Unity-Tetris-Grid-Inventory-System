using System;
using UnityEngine;

namespace ProjectFiles.TetrisGrid
{
    [Serializable]
    public struct InventoryItemToPlace
    {
        public ItemDataScriptableObject ItemPreset;
        public Vector2Int Position;
    }
}