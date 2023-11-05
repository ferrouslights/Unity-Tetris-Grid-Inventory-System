using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectFiles.TetrisGrid
{
    public class GridInteractableManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private ItemGrid _grid;
        
        private GridInventoryController _controller;

        public void OnPointerEnter(PointerEventData eventData) => _controller.FocusGrid(_grid);

        public void OnPointerExit(PointerEventData eventData) => _controller.UnfocusGrid(_grid);

        private void Start() => _controller = GridInventoryController.Instance;
    }
}