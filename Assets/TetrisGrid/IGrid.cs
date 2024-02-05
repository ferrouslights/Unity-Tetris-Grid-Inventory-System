namespace ProjectFiles.TetrisGrid
{
    public interface IGrid
    {
        public bool CheckItemFits(InventoryItem item, int x, int y);
        public InventoryItem GrabItem(int x, int y);
        public void PlaceItem(InventoryItem item, int x, int y);
        public IGrid InitializeGrid(GridParameters gridParameters);
        public InventoryItem GetItem(int x, int y);
    }
}