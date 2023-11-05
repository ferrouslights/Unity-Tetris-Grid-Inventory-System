#if UNITY_EDITOR
using System.Collections.Generic;
using ProjectFiles.TetrisGrid;
using UnityEngine;

public class DemoInventoryInitializer : MonoBehaviour, IGetInventoryItems
{
    [SerializeField] private InventoryItem _itemPrefab;
    [SerializeField] private List<InventoryItemToPlace> _itemsToPlace;

    public List<InventoryItemToPlace> GetInventoryItems() => _itemsToPlace;

    public InventoryItem GetItemPrefab() => _itemPrefab;
}
#endif