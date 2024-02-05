using UnityEngine;

namespace ProjectFiles.TetrisGrid
{
    public class GridInventoryController : MonoBehaviour
    {
        public static GridInventoryController Instance;
        [HideInInspector]
        public ItemGrid ActiveGrid;
        
        private IInteractInput _interactInput;
        private ICursorInput _cursorInput;
        private IUseInput _useInput;
        
        private InventoryItem _currentlySelectedInventoryItem;
        private Vector2Int _currentlySelectedInventoryItemLastPosition;
        private Orientation _currentlySelectedInventoryItemLastOrientation;
        private ItemGrid _currentlySelectedInventoryItemLastGrid;

        public void FocusGrid(ItemGrid grid)
        {
            ActiveGrid = grid;
        }
        
        public void UnfocusGrid(ItemGrid grid)
        {
            if (ActiveGrid != grid)
            {
                return;
            }
            
            ActiveGrid = null;
        }
        
        private void Awake()
        {
            if (Instance != null)
            {
                return;
            }
            Instance = this;
            _interactInput = GetComponent<IInteractInput>();
            _cursorInput = GetComponent<ICursorInput>();
            _useInput = GetComponent<IUseInput>();
        }
        
        private void Update()
        {
            if (ActiveGrid == null || !ActiveGrid.enabled)
            {
                return;
            }
            
            if (_currentlySelectedInventoryItem != null)
            {
                _currentlySelectedInventoryItem.RectTransform.position = _cursorInput.CursorPosition;
            }

            if (_useInput is { Use: true })
            {
                if (_currentlySelectedInventoryItem != null)
                {
                    _currentlySelectedInventoryItem.Rotate();
                    _currentlySelectedInventoryItemLastOrientation = _currentlySelectedInventoryItem.Orientation;
                    Debug.Log($"Item Orientation is now: {_currentlySelectedInventoryItem.Orientation}");
                }
                _useInput.Use = false;
                return;
            }

            if (_interactInput is not { Interact: true })
            {
                return;
            }
            _interactInput.Interact = false;

            var pos = ActiveGrid.GetTiledGridPosition(_cursorInput.CursorPosition);

            if (_currentlySelectedInventoryItem == null)
            {
                GrabItem(pos);
                _currentlySelectedInventoryItemLastGrid = ActiveGrid;
                return;
            }

            if (!ActiveGrid.CheckItemFits(_currentlySelectedInventoryItem, pos.x, pos.y))
            {
                while (_currentlySelectedInventoryItem.Orientation != _currentlySelectedInventoryItemLastOrientation)
                {
                    Debug.Log("Rotating!");
                    _currentlySelectedInventoryItem.Rotate();
                    Debug.Log($"New rotation: {_currentlySelectedInventoryItem.Orientation}");
                }
                PlaceItem(_currentlySelectedInventoryItemLastPosition, _currentlySelectedInventoryItemLastGrid);
                return;
            }
            
            PlaceItem(pos);
        }

        private void GrabItem(Vector2Int pos)
        {
            _currentlySelectedInventoryItem = ActiveGrid.GrabItem(pos.x, pos.y);
            _currentlySelectedInventoryItemLastPosition = pos;
        }

        private void PlaceItem(Vector2Int pos, ItemGrid gridOverride = null)
        {
            var grid = gridOverride != null ? gridOverride : ActiveGrid;
            if (gridOverride != null && !gridOverride.CheckItemFits(_currentlySelectedInventoryItem, pos.x, pos.y))
            {
                var attempt = 0;
                
                while (attempt < 4)
                {
                    _currentlySelectedInventoryItem.Rotate();
                    if (gridOverride.CheckItemFits(_currentlySelectedInventoryItem, pos.x, pos.y))
                    {
                        grid.PlaceItem(_currentlySelectedInventoryItem, pos.x, pos.y);
                        _currentlySelectedInventoryItem = null;
                        break;
                    }
                    attempt++;
                }
                return;
            }
            
            grid.PlaceItem(_currentlySelectedInventoryItem, pos.x, pos.y);
            _currentlySelectedInventoryItem = null;
        }
    }
}