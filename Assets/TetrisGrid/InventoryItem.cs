using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectFiles.TetrisGrid
{
    public class InventoryItem : MonoBehaviour, IInventoryItemData
    {
        public int Width => _itemData.Width;
        public int Height => _itemData.Height;
        public bool[] OccupiedGrid => _itemData.OccupiedGrid;
        public bool[] OccupiedGridRotated => _itemData.OccupiedGridRotated;
        public Sprite Sprite => _itemData.Sprite;
        public Orientation Orientation => _itemData.Orientation;
        
        public RectTransform RectTransform;
        public Vector2Int Position;
        
        [SerializeField] private Image _image;

        private IInventoryItemData _itemData;

        public void Initialize(IInventoryItemData itemData)
        {
            _itemData = itemData;
            RectTransform.sizeDelta = new Vector2(_itemData.Width * 32, _itemData.Height * 32);
            _image.sprite = _itemData.Sprite;
        }

        public bool[] GetGridRow(int row)
        {
            var gridRow = new bool[Width];
            for (var i = 0; i < Width; i++)
            {
                gridRow[i] = OccupiedGridRotated[row * Width + i];
            }

            return gridRow;
        }
        

        public void Rotate()
        {
            _itemData.Rotate();
            RectTransform.eulerAngles = new Vector3(0, 0, -(int)_itemData.Orientation);
            RectTransform.pivot = GetPivotFromOrientation(_itemData.Orientation);
        }
      
        private static Vector2 GetPivotFromOrientation(Orientation orientation) =>
            orientation switch
            {
                Orientation.Up => new Vector2(0, 1),
                Orientation.Right => new Vector2(0, 0),
                Orientation.Down => new Vector2(1, 0),
                Orientation.Left => new Vector2(1, 1),
                _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
            };

        private void Awake() => RectTransform = GetComponent<RectTransform>();
    }
}