using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IDropHandler
{
    [field: SerializeField] public int ItemListIndex { get; set; }
    [field: SerializeField] public bool IsDragDrop { get; set; }
    [field: SerializeField] public InventoryItemSO Item { get; set; }
    [field: SerializeField] public Image Icon { get; private set; }
    [field: SerializeField] public GameObject Focus { get; private set; }
    [field: SerializeField] public GameObject RequiredLevelFrame { get; private set; }
    [field: SerializeField] public TextMeshProUGUI RequiredLevelText { get; private set; }

    private IUIManagerInventory _IUIManagerInventory;
    private Inventory _inventory;
    private CharacterStats _characterStats;
    
    private const int ITEM_FOCUSED_NULL_INDEX = -1;

    public void Initialize(Inventory inventory, IUIManagerInventory iUIManagerInventory, CharacterStats characterStats)
    {
        _inventory = inventory;
        _IUIManagerInventory = iUIManagerInventory;
        _characterStats = characterStats;

        GetComponent<DragDropItems>().Initialize(_inventory);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Item == null)
        {
            _IUIManagerInventory.ShowInventoryStats();
            _inventory.FocusedCurrentInventorySlot?.Focus.SetActive(false);
            _inventory.FocusedCurrentInventorySlot = null;
            _inventory.ItemFocusedIndex = ITEM_FOCUSED_NULL_INDEX;
            _characterStats.SelectFocusedSlot(ITEM_FOCUSED_NULL_INDEX);
            return;
        }

        _inventory.FocusedCurrentInventorySlot?.Focus.SetActive(false);
        _inventory.FocusedCurrentInventorySlot = this;
        Focus.SetActive(true);
        _characterStats.SelectFocusedSlot((int)Item.ItemType);
        _inventory.ItemFocusedIndex = ItemListIndex;
        ShowItemInfo();
    }

    private void ShowItemInfo()
    {
        _IUIManagerInventory.ShowInventoryItemDescription();
        _IUIManagerInventory.SetItemName(Item.ItemName);
        _IUIManagerInventory.SetItemDescription(GetDescription());
        _IUIManagerInventory.SetItemType(Item.ItemType.ToString());
        _IUIManagerInventory.SetItemRequiredLevel(Item.RequiredLevel);
        _IUIManagerInventory.SetItemPrice(Item.Price);
    }

    private string GetDescription()
    {
        string lang = Lean.Localization.LeanLocalization.GetFirstCurrentLanguage();
        string description;
        switch (lang)
        {
            case "English": description = Item.Description; break;
            case "Ukraine": description = Item.DescriptionUkraine; break;
            case "Russian": description = Item.DescriptionRussian; break;
            default: description = Item.Description; break;
        }
        if (description == "")
        {
            description = Item.Description;
            Tools.LogError(lang + ": description of " + Item.ItemName + " is empty.");
        }
        return description;
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlot dragItem = eventData.pointerDrag?.GetComponent<InventorySlot>();
        if (dragItem.IsDragDrop)
        {
            _inventory.SwapItems(dragItem.ItemListIndex, ItemListIndex);
        }
    }

}
