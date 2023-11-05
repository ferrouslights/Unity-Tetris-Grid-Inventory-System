using System;
using UnityEngine;

namespace ProjectFiles.TetrisGrid
{
    public class ItemGrid : MonoBehaviour
    {
        [SerializeField] private Vector2Int _gridSquareSize = new(32,32);
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] RectTransform _rectTransform;
        [SerializeField] private InventoryItem _itemPrefab;
        
        private Vector2 _positionOnGrid;
        private Vector2Int _gridPosition;
        private InventoryItem[,] _gridData;

        public void Start()
        {
            InitializeGrid(_gridSize.x, _gridSize.y);
            var item = Instantiate(_itemPrefab, transform);
            PlaceItem(item, 5,5);
            
            var item2 = Instantiate(_itemPrefab, transform);
            PlaceItem(item2, 2,2);
        }

        public Vector2Int GetTiledGridPosition(Vector2 mousePosition)
        {
            _positionOnGrid.x = mousePosition.x - _rectTransform.position.x;
            _positionOnGrid.y = mousePosition.y - _rectTransform.position.y;
            
            _gridPosition.x = Mathf.FloorToInt(_positionOnGrid.x / _gridSquareSize.x);
            _gridPosition.y = -Mathf.FloorToInt(_positionOnGrid.y / _gridSquareSize.y) - 1;

            return _gridPosition;
        }

        public InventoryItem GrabItem(int x, int y)
        {
            var item = _gridData[x, y];
            // factor in item position
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

        public bool CheckItemFits(InventoryItem item, int x, int y)
        {
            if (x + item.Width > _gridSize.x || y + item.Height > _gridSize.y)
            {
                return false;
            }
            
            for (var i = 0; i < item.Height; i++)
            {
                var gridRow = item.GetGridRow(i);
                for (var j = 0; j < item.Width; j++)
                {
                    if (gridRow[j] && _gridData[x + j, y + i] != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        
        public void PlaceItem(InventoryItem item, int x, int y)
        {
            item.RectTransform.SetParent(_rectTransform);
            item.Position = new Vector2Int(x, y);
            AssignTiles(item, item.Position.x, item.Position.y);
            var position = new Vector2(x * _gridSquareSize.x, -y * _gridSquareSize.y);
            item.RectTransform.localPosition = position;
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

        private void InitializeGrid(int x, int y)
        {
            _rectTransform.sizeDelta = new Vector2(x * _gridSquareSize.x, y * _gridSquareSize.y);
            _gridData = new InventoryItem[x, y];
        }

    }
}