using UnityEngine;

namespace ProjectFiles.TetrisGrid
{
    public class ItemGrid : MonoBehaviour
    {
        [SerializeField] private Vector2Int _gridSquareSize = new(32, 32);
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] RectTransform _rectTransform;
        
        private Vector2 _positionOnGrid;
        private Vector2Int _gridPosition;
        
        private IGrid _gridBase;
        private IGetInventoryItems _inventoryItems;

        public Vector2Int GetTiledGridPosition(Vector2 mousePosition)
        {
            _positionOnGrid.x = mousePosition.x - _rectTransform.position.x;
            _positionOnGrid.y = mousePosition.y - _rectTransform.position.y;

            _gridPosition.x = Mathf.FloorToInt(_positionOnGrid.x / _gridSquareSize.x);
            _gridPosition.y = -Mathf.FloorToInt(_positionOnGrid.y / _gridSquareSize.y) - 1;

            return _gridPosition;
        }

        public InventoryItem GrabItem(int x, int y) => _gridBase.GrabItem(x, y);

        public bool CheckItemFits(InventoryItem item, int x, int y) => _gridBase.CheckItemFits(item, x, y);
        
        public void PlaceItem(InventoryItem item, int x, int y)
        {
            item.RectTransform.SetParent(_rectTransform);
            item.Position = new Vector2Int(x, y);
            _gridBase.PlaceItem(item, x, y);
            var position = new Vector2(x * _gridSquareSize.x, -y * _gridSquareSize.y);
            item.RectTransform.localPosition = position;
        }

        private void InitializeGrid(int x, int y)
        {
            _rectTransform.sizeDelta = new Vector2(x * _gridSquareSize.x, y * _gridSquareSize.y);
            var gridData = new InventoryItem[x, y];
            var gridParameters = new GridParameters
            {
                GridSize = _gridSize,
                GridSquareSize = _gridSquareSize,
                GridInventoryData = gridData
            };
            _gridBase = new GridBase().InitializeGrid(gridParameters);

            if (_inventoryItems == null)
            {
                return;
            }

            var items = _inventoryItems.GetInventoryItems();
            var itemPrefab = _inventoryItems.GetItemPrefab();

            foreach (var item in items)
            {
                var itemObject = Instantiate(itemPrefab, _rectTransform);
                itemObject.Initialize(item.ItemPreset.ItemData);

                if (CheckItemFits(itemObject, item.Position.x, item.Position.y))
                {
                    PlaceItem(itemObject, item.Position.x, item.Position.y);
                    continue;
                }

                Destroy(itemObject.gameObject);
            }
        }
        
        private void Start() => InitializeGrid(_gridSize.x, _gridSize.y);
        private void Awake() => _inventoryItems = GetComponent<IGetInventoryItems>();
    }
}