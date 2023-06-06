using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/New Item")]
public class InventoryItemSO : ScriptableObject
{
    [field: SerializeField] public GameObject ItemPrefab { get; private set; }

    [field: SerializeField, Space(10)] public int ItemID { get; private set; }
    [field: SerializeField] public string ItemName { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public ItemTypes ItemType { get; private set; }
    [field: SerializeField] public int ItemLevel { get; private set; }
    [field: SerializeField] public int RequiredLevel { get; private set; }
    [field: SerializeField] public int Price { get; private set; }
    
    [field: SerializeField,
    Header("Modifiers: ----------------------------"),
    Header("0 = Fire, 1 = Water, 2 = Earth"),
    Header("3 = Wind, 4 = Metal, 5 = Lightning"),
    Header("6 = Life, 7 = Death, 8 = Space")]
    public int[] AttackValues { get; private set; }
    [field: SerializeField] public int[] DefenseValues { get; private set; }

    [field: SerializeField, TextArea(5, 10)] public string Description { get; private set; }
    [field: SerializeField, TextArea(5, 10)] public string DescriptionUkraine { get; private set; }
    [field: SerializeField, TextArea(5, 10)] public string DescriptionRussian { get; private set; }
}

public enum ItemTypes
{
        Sword = 0, Hat = 1, Armor = 2, Shield = 3, Boots = 4,
        Axe = 5, Staff = 6, Gloves = 7, Amulet = 8, Other = 9,
        Ring = 10, Plant = 11, Crystal = 12, EnemyPart = 13
}
