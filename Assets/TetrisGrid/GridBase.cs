using UnityEngine;

namespace ProjectFiles.TetrisGrid
{
    public class GridBase : IGrid
    {
        protected Vector2Int _gridSize;
        protected Vector2Int _gridSquareSize;
        protected InventoryItem[,] _gridData;


        public InventoryItem GetItem(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _gridSize.x || y >= _gridSize.y)
            {
                return null;
            }
            
            return _gridData[x, y];
        }

        public bool CheckItemFits(InventoryItem item, int x, int y)
        {
            // check if grid position is within bounds
            if (x < 0 || y < 0 || x >= _gridSize.x || y >= _gridSize.y)
            {
                Debug.Log("Out of bounds!");
                return false;
            }

            // Check if items bounds are within grid bounds
            if (x + item.Width > _gridSize.x || y + item.Height > _gridSize.y)
            {
                Debug.Log("Item out of bounds!");
                return false;
            }

            // Check if item overlaps with other items
            var sliceOfGrid = new InventoryItem[item.Width, item.Height];
            for (var i = 0; i < item.Height; i++)
            {
                for (var j = 0; j < item.Width; j++)
                {
                    sliceOfGrid[j, i] = _gridData[x + j, y + i];
                }
            }

            for (var i = 0; i < item.Height; i++)
            {
                var gridRow = item.GetGridRow(i);
                for (var j = 0; j < item.Width; j++)
                {
                    if (gridRow[j] && sliceOfGrid[j, i] != null)
                    {
                        Debug.Log("Item overlaps!");
                        return false;
                    }
                }
            }

            return true;
        }

        public InventoryItem GrabItem(int x, int y)
        {
            var item = _gridData[x, y];
            if (item == null)
            {
                return item;
            }

            for (var i = 0; i < item.Height; i++)
            {
                var gridRow = item.GetGridRow(i);
                for (var j = 0; j < item.Width; j++)
                {
                    if (gridRow[j])
                    {
                        _gridData[item.Position.x + j, item.Position.y + i] = null;
                    }
                }
            }

            return item;
        }

        private void AssignTiles(InventoryItem item, int x, int y, bool remove = false)
        {
            if (item == null)
            {
                return;
            }

            for (var i = 0; i < item.Height; i++)
            {
                var gridRow = item.GetGridRow(i);
                for (var j = 0; j < item.Width; j++)
                {
                    if (gridRow[j])
                    {
                        _gridData[x + j, y + i] = remove ? null : item;
                    }
                }
            }
        }

        public void PlaceItem(InventoryItem item, int x, int y)
        {
            AssignTiles(item, item.Position.x, item.Position.y);
        }

        public IGrid InitializeGrid(GridParameters gridParameters)
        {
            _gridSize = gridParameters.GridSize;
            _gridSquareSize = gridParameters.GridSquareSize;
            _gridData = gridParameters.GridInventoryData;
            return this;
        }
    }
}