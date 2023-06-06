using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Loot/New LootItemList")]
public class LootItemListSO : ScriptableObject
{
    [field: SerializeField, Range (1, 100)] public int TotalDropProbability { get; private set; }
    [field: SerializeField, Header("Sort items from smallest[0] to largest[N] probability: \n Items with 100% are always instantiated. \n They are not included in itemAmount.")] public List<LootItem> ItemList { get; private set; }
    [SerializeField, TextArea] private string _description;
}

[System.Serializable]
public class LootItem
{
    public InventoryItemSO ItemSO;
    public int Probability;
}

