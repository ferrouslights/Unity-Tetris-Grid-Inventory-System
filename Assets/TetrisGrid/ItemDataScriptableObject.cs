using UnityEngine;

namespace ProjectFiles.TetrisGrid
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
    public class ItemDataScriptableObject : ScriptableObject, IGetItemData
    {
        public IInventoryItemData ItemData => new ItemData(_itemData);
        
        [SerializeField] private ItemData _itemData;
    }
}