using System.Collections.Generic;

namespace ProjectFiles.TetrisGrid
{
    public interface IGetInventoryItems
    {
        public List<InventoryItemToPlace> GetInventoryItems();
        public InventoryItem GetItemPrefab();
    }
}