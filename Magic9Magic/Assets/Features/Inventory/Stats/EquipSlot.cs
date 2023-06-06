using UnityEngine;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour, IPointerClickHandler
{
    private IUIManagerInventory _IUIManagerInventory;
    public int Index { get; set; }
    public InventoryItemSO Item { get; set; }
    private CharacterStats _characterStats;
    private Inventory _inventory;

    private const int ITEM_FOCUSED_NULL_INDEX = -1;
    
    public void Initialize(CharacterStats characterStats, IUIManagerInventory iUIManagerInventory, Inventory inventory)
    {
        _characterStats = characterStats;
        _IUIManagerInventory = iUIManagerInventory;
        _inventory = inventory;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _inventory.UnfocusCurrentItem();

        if (Item == null)
        {
            _characterStats.SelectFocusedItem(ITEM_FOCUSED_NULL_INDEX);
            _IUIManagerInventory.ShowInventoryStats();
            return;   
        }
        _characterStats.SelectFocusedItem(Index);

        ShowItemInfo();
    }

    public void ShowItemInfo()
    {
        _IUIManagerInventory.ShowInventoryItemDescription();
        _IUIManagerInventory.SetItemName(Item.ItemName);
        _IUIManagerInventory.SetItemDescription(Item.Description);
        _IUIManagerInventory.SetItemType(Item.ItemType.ToString());
        _IUIManagerInventory.SetItemRequiredLevel(Item.RequiredLevel);
        _IUIManagerInventory.SetItemPrice(Item.Price);
    }

}
