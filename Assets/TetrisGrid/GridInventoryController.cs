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
                }
                _useInput.Use = false;
                return;
            }

            if (_interactInput is not { Interact: true })
            {
                return;
            }

            var pos = ActiveGrid.GetTiledGridPosition(_cursorInput.CursorPosition);

            if (_currentlySelectedInventoryItem == null)
            {
                GrabItem(pos);
                return;
            }

            if (!ActiveGrid.CheckItemFits(_currentlySelectedInventoryItem, pos.x, pos.y))
            {
                while (_currentlySelectedInventoryItem.Orientation != _currentlySelectedInventoryItemLastOrientation)
                {
                    _currentlySelectedInventoryItem.Rotate();
                }
                PlaceItem(_currentlySelectedInventoryItemLastPosition);
                return;
            }
            
            PlaceItem(pos);
        }

        private void GrabItem(Vector2Int pos)
        {
            _currentlySelectedInventoryItem = ActiveGrid.GrabItem(pos.x, pos.y);
            _currentlySelectedInventoryItemLastPosition = pos;
        }

        private void PlaceItem(Vector2Int pos)
        {
            ActiveGrid.PlaceItem(_currentlySelectedInventoryItem, pos.x, pos.y);
            _currentlySelectedInventoryItem = null;
        }
    }
}