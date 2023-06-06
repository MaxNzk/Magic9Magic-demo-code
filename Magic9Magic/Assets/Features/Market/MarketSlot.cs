using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MarketSlot : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] public int ItemListIndex { get; set; }
    [field: SerializeField] public bool IsDragDrop { get; set; }
    [field: SerializeField] public InventoryItemSO Item { get; set; }
    [field: SerializeField] public Image Icon { get; private set; }
    [field: SerializeField] public GameObject Focus { get; private set; }
    [field: SerializeField] public GameObject RequiredLevelFrame { get; private set; }
    [field: SerializeField] public TextMeshProUGUI RequiredLevelText { get; private set; }

    private Market _market;
    private IUIManagerMarket _IUIManagerMarket;
    private int _storeMarkupPercentage;

    public void Initialize(Market market, IUIManagerMarket iUIManagerMarket, int storeMarkupPercentage)
    {
        _market = market;
        _IUIManagerMarket = iUIManagerMarket;
        _storeMarkupPercentage = storeMarkupPercentage;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Item == null)
            return;

        _market.FocusedCurrentMarketSlot?.Focus.SetActive(false);
        _market.FocusedCurrentMarketSlot = this;
        Focus.SetActive(true);
        _market.ItemFocusedIndex = ItemListIndex;
        ShowItemInfo();
    }

    private void ShowItemInfo()
    {
        _IUIManagerMarket.SetItemName(Item.ItemName);
        _IUIManagerMarket.SetItemDescription(GetDescription());
        _IUIManagerMarket.SetItemType(Item.ItemType.ToString());
        _IUIManagerMarket.SetItemRequiredLevel(Item.RequiredLevel);
        _IUIManagerMarket.SetItemPrice(GetItemPriceWithMarkupPercentage(Item.Price, _storeMarkupPercentage));
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

    private int GetItemPriceWithMarkupPercentage(int price, int storeMarkupPercentage)
    {
        int newPrice = Mathf.RoundToInt(price * (1 + (float)storeMarkupPercentage / 100));
        return newPrice;
    }
}
