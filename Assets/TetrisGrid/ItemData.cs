using System;
using UnityEngine;

namespace ProjectFiles.TetrisGrid
{
    [Serializable]
    public class ItemData : IInventoryItemData
    {
        public int Width => _width;
        public int Height => _height;
        public bool[] OccupiedGrid => _occupiedGrid;
        public bool[] OccupiedGridRotated => _occupiedGridRotated;
        public Sprite Sprite => _sprite;
        public Orientation Orientation => _orientation;

        public static Orientation[] Orientations =
            { Orientation.Up, Orientation.Right, Orientation.Down, Orientation.Left };

        [SerializeField] private int _width;

        [SerializeField] private int _height;

        [SerializeField] private bool[] _occupiedGrid;
        private bool[] _occupiedGridRotated;

        [SerializeField] private Sprite _sprite;

        [SerializeField] private Orientation _orientation;
        private bool _rotatedAtLeastOnce;
        
        public ItemData(int width, int height, bool[] occupiedGrid, Sprite sprite)
        {
            _width = width;
            _height = height;
            _occupiedGrid = occupiedGrid;
            _sprite = sprite;
            _orientation = Orientation.Up;
            _occupiedGridRotated = _occupiedGrid.Clone() as bool[];
        }
        
        public ItemData(ItemData itemData) : this(itemData.Width, itemData.Height, itemData.OccupiedGrid, itemData.Sprite) { }

        public void Rotate()
        {
            _occupiedGridRotated = RotateGrid90Degrees
            (
                _rotatedAtLeastOnce ? _occupiedGridRotated : _occupiedGrid,
                _width,
                _height
            );
            
            (_width, _height) = (_height, _width);

            _orientation = GetNextOrientation(_orientation, true);
            _rotatedAtLeastOnce = true;
        }

        public static Orientation GetNextOrientation(Orientation current, bool isClockwise)
        {
            var index = Array.IndexOf(Orientations, current);
            if (isClockwise)
            {
                index++;
                if (index >= Orientations.Length)
                {
                    index = 0;
                }
            }
            else
            {
                index--;
                if (index < 0)
                {
                    index = Orientations.Length - 1;
                }
            }

            return Orientations[index];
        }

        public static bool[] RotateGrid90Degrees(bool[] originalGrid, int width, int height)
        {
            if (originalGrid.Length != width * height)
            {
                throw new ArgumentException("Invalid grid dimensions.");
            }

            var rotatedGrid = new bool[originalGrid.Length];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var sourceIndex = y * width + x;
                    var targetIndex = x * height + (height - 1 - y);
                    rotatedGrid[targetIndex] = originalGrid[sourceIndex];
                }
            }

            return rotatedGrid;
        }
    }
}