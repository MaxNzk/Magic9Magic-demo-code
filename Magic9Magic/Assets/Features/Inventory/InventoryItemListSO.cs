using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/New ItemList")]
public class InventoryItemListSO : ScriptableObject
{
    [field: SerializeField] public List<InventoryItemSO> ItemList { get; private set; }
    [SerializeField, TextArea] private string _description;
}
