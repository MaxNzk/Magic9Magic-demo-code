using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Market : MonoBehaviour
{
    [SerializeField] private InventoryItemListSO _marketItemList;
    [SerializeField, Range(0, 100)] private int _markupPercentage;

    [Space(10)]
    [SerializeField] private bool _areItemsUpdatableAfterBuying;
    [SerializeField] private int _itemUpdateCostPercentage;
    [SerializeField] private int _costRoundToMultiplesOf;
    private int _itemUpdateCost;
    [SerializeField] private int _firstNonUpdatedSlotsAmount;

    [Space(10)]
    [Header("Item Lists with levels ---------------------")]
    [SerializeField] private List<InventoryItemSO> _silverRingList = new List<InventoryItemSO>();
    [SerializeField] private List<InventoryItemSO> _goldenRingList = new List<InventoryItemSO>();
    [SerializeField] private List<InventoryItemSO> _blackRingList = new List<InventoryItemSO>();
    [SerializeField] private List<InventoryItemListSO> _amuletLists = new List<InventoryItemListSO>();
    [SerializeField] private List<InventoryItemListSO> _armorLists = new List<InventoryItemListSO>();
    [SerializeField] private List<InventoryItemListSO> _axeLists = new List<InventoryItemListSO>();
    [SerializeField] private List<InventoryItemListSO> _bootsLists = new List<InventoryItemListSO>();
    [SerializeField] private List<InventoryItemListSO> _glovesLists = new List<InventoryItemListSO>();
    [SerializeField] private List<InventoryItemListSO> _hatLists = new List<InventoryItemListSO>();
    [SerializeField] private List<InventoryItemListSO> _otherLists = new List<InventoryItemListSO>();
    [SerializeField] private List<InventoryItemListSO> _shieldLists = new List<InventoryItemListSO>();
    [SerializeField] private List<InventoryItemListSO> _staffLists = new List<InventoryItemListSO>();
    [SerializeField] private List<InventoryItemListSO> _swordLists = new List<InventoryItemListSO>();
    private List<InventoryItemListSO> _itemFocusedLists = new List<InventoryItemListSO>();

    private List<MarketSlot> _slotList = new List<MarketSlot>();

    [Space(10)]
    [SerializeField] private int _slotAmount;
    
    [Space(10)]
    [Header("UI -----------------------------------------")]
    [SerializeField] private RectTransform _slotsContainer;
    [SerializeField] private GameObject _marketSlotPrefab;
    [SerializeField] private Button _updateButton;
    [SerializeField] private TextMeshProUGUI _updateButtonText;
    [SerializeField] private TextMeshProUGUI _updateButtonTextTmp;
    
    public MarketSlot FocusedCurrentMarketSlot { get; set; }
    private int _itemFocusedIndex;
    public int ItemFocusedIndex
    {
        get => _itemFocusedIndex; 
        set
        {
            _itemFocusedIndex = value;

            if (_itemFocusedIndex < _firstNonUpdatedSlotsAmount)
            {
                // Debug.Log("It is a first non-updated slot | return;");
                _updateButton.interactable = false;
                return;
            }

            _updateButtonTextTmp.gameObject.SetActive(false); // here, due to the need to apply the translation in the active state.
            _itemUpdateCost = (_slotList[_itemFocusedIndex].Item.Price * _itemUpdateCostPercentage / 100).RoundToMultiples(_costRoundToMultiplesOf);
            _updateButtonText.text = _updateButtonTextTmp.text + " (" + _itemUpdateCost.ToString() + ")";
            _updateButton.interactable = true;
        }
    }

    private Inventory _inventory;
    private Wallet _wallet;
    private IUIManagerMarket _IUIManagerMarket;
    private PlayerSettings _playerSettingsSO;
    private int _playerLevel;

    public void Initialize(Inventory inventory, Wallet wallet, IUIManagerMarket iUIManagerMarket, PlayerSettings playerSettingsSO)
    {
        _inventory = inventory;
        _wallet = wallet;
        _IUIManagerMarket = iUIManagerMarket;
        _playerSettingsSO = playerSettingsSO;

        _playerLevel = _playerSettingsSO.Level;

        CreateSlots();
        ChooseSellingItem();
        LoadItemsFromListSO();
        ActivateFirstItem();
    }

    private void CreateSlots()
    {
        for (int i = 0; i < _slotAmount; i++)
        {
            GameObject item = Instantiate(_marketSlotPrefab);
            item.transform.SetParent(_slotsContainer, false);
            MarketSlot itemSlotScript = item.GetComponent<MarketSlot>();
            itemSlotScript.ItemListIndex = i;
            itemSlotScript.IsDragDrop = false;
            itemSlotScript.Initialize(this, _IUIManagerMarket, _markupPercentage);
            _slotList.Add(itemSlotScript);
        }
    }

    private void ChooseSellingItem()
    {
        if (_playerSettingsSO.IsLevelChanged == false)
            return;
        
        _playerSettingsSO.IsLevelChanged = false;

        RenewRingItems();

        _itemFocusedLists = _swordLists;
        RenewItems(3, 2);
        _itemFocusedLists = _shieldLists;
        RenewItems(5, 2);
        _itemFocusedLists = _staffLists;
        RenewItems(7, 2);
        _itemFocusedLists = _hatLists;
        RenewItems(9, 2);
        _itemFocusedLists = _glovesLists;
        RenewItems(11, 2);
        _itemFocusedLists = _bootsLists;
        RenewItems(13, 2);
        _itemFocusedLists = _axeLists;
        RenewItems(15, 2);
        _itemFocusedLists = _armorLists;
        RenewItems(17, 2);
        _itemFocusedLists = _amuletLists;
        RenewItems(19, 2);
        _itemFocusedLists = _otherLists;
        RenewItems(21, 4);
    }

    private void RenewRingItems()
    {
        int index = (_playerLevel - 1) / 3;

        if (_silverRingList[index].RequiredLevel >= _playerLevel)
        {
            _marketItemList.ItemList[0] = _silverRingList[index];
        }
        else
        {
            _marketItemList.ItemList[0] = null;
        }

        if (_goldenRingList[index].RequiredLevel >= _playerLevel)
        {
            _marketItemList.ItemList[1] = _goldenRingList[index];
        }
        else
        {
            _marketItemList.ItemList[1] = null;
        }
        
        if (_blackRingList[index].RequiredLevel >= _playerLevel)
        {
            _marketItemList.ItemList[2] = _blackRingList[index];
        }
        else
        {
            _marketItemList.ItemList[2] = null;
        }
    }

    private void RenewItems(int startPosition, int amount)
    {
        int index = Mathf.Clamp(_playerLevel - 1, 0, _itemFocusedLists.Count - 1);
        bool isTheSameItem = true;
        int randomNewItemIndex = Random.Range(0, _itemFocusedLists[index].ItemList.Count);
        _marketItemList.ItemList[startPosition] = _itemFocusedLists[index].ItemList[randomNewItemIndex];
        // Debug.Log("ItemName[" + startPosition.ToString() + "] = " + _itemFocusedLists[index].ItemList[randomNewItemIndex].name);

        for (int i = startPosition + 1; i < startPosition + amount; i++)
        {
            isTheSameItem = true;
            while (isTheSameItem)
            {
                isTheSameItem = false;
                randomNewItemIndex = Random.Range(0, _itemFocusedLists[index].ItemList.Count);
                for (int k = startPosition; k < i; k++)
                {
                    // Debug.Log("_marketItemList.ItemList[k].name = " + _marketItemList.ItemList[k].name);
                    // Debug.Log("_itemFocusedLists[index].ItemList[randomNewItemIndex].name = " + _itemFocusedLists[index].ItemList[randomNewItemIndex].name);
                    if (_marketItemList.ItemList[k].name == _itemFocusedLists[index].ItemList[randomNewItemIndex].name)
                    {
                        isTheSameItem = true;
                        // Debug.Log("isTheSameItem [" + k.ToString() + "] = " + _itemFocusedLists[index].ItemList[randomNewItemIndex].name);
                        break;
                    }
                }
            }
            _marketItemList.ItemList[i] = _itemFocusedLists[index].ItemList[randomNewItemIndex];
            // Debug.Log("ItemName[" + i.ToString() + "] = " + _itemFocusedLists[index].ItemList[randomNewItemIndex].name);
        }
    }

    private void LoadItemsFromListSO()
    {
        for (int i = 0; i < _marketItemList.ItemList.Count; i++)
        {
            if (i == _slotAmount)
                break;
            
            if (_marketItemList.ItemList[i] == null)
                continue;

            _slotList[i].Item = _marketItemList.ItemList[i];
            _slotList[i].Icon.sprite = _marketItemList.ItemList[i].Icon;
            _slotList[i].Icon.enabled = true;
            _slotList[i].RequiredLevelFrame.SetActive(true);
            _slotList[i].RequiredLevelText.text = _marketItemList.ItemList[i].RequiredLevel.ToString();
            _slotList[i].RequiredLevelText.enabled = true;
        }
    }

    private void ActivateFirstItem()
    {
        for (int i = 0; i < _slotList.Count; i++)
        {
            if (_slotList[i].Item != null)
            {
                _slotList[i].OnPointerClick(null);
                break;
            }
        }
    }

    // Canvas > UILayerPopup > Market > BottonFrame > UpdateButton
    public void UpdateButton()
    {
        if (_slotList[_itemFocusedIndex].Item.ItemType == ItemTypes.Ring)
        {
            // Debug.Log("It is a ring (first non-updated slots) | return;");
            return;
        }

        if (_wallet.TryPayWithGold(_itemUpdateCost))
        {
            UpdateItem();
        }
        else
        {
            Tools.Log("Not enough gold");
        }
    }

    private void UpdateItem()
    {
        SetItemFocusedLists();
        int randomNewItemIndex = Random.Range(0, _itemFocusedLists[_playerLevel - 1].ItemList.Count);
        string previousItemName = _slotList[_itemFocusedIndex].Item.name;
        // Debug.Log("previousItemName = " + previousItemName);
        // Debug.Log("newItemName = " + _itemFocusedLists[_playerLevel - 1].ItemList[randomNewItemIndex].name);
        while (previousItemName == _itemFocusedLists[_playerLevel - 1].ItemList[randomNewItemIndex].name)
        {
            randomNewItemIndex = Random.Range(0, _itemFocusedLists[_playerLevel - 1].ItemList.Count);
            // Debug.Log("newItemName = " + _itemFocusedLists[_playerLevel - 1].ItemList[randomNewItemIndex].name);
        }

        _marketItemList.ItemList[_itemFocusedIndex] = _itemFocusedLists[_playerLevel - 1].ItemList[randomNewItemIndex];

        _slotList[_itemFocusedIndex].Item = _itemFocusedLists[_playerLevel - 1].ItemList[randomNewItemIndex];
        _slotList[_itemFocusedIndex].Icon.sprite = _itemFocusedLists[_playerLevel - 1].ItemList[randomNewItemIndex].Icon;
        _slotList[_itemFocusedIndex].RequiredLevelText.text = _itemFocusedLists[_playerLevel - 1].ItemList[randomNewItemIndex].RequiredLevel.ToString();

        _slotList[_itemFocusedIndex].OnPointerClick(null);
    }

    private void SetItemFocusedLists()
    {
        if (_slotList[_itemFocusedIndex].Item.ItemType == ItemTypes.Amulet)
            _itemFocusedLists = _amuletLists;
        if (_slotList[_itemFocusedIndex].Item.ItemType == ItemTypes.Armor)
            _itemFocusedLists = _armorLists;
        if (_slotList[_itemFocusedIndex].Item.ItemType == ItemTypes.Axe)
            _itemFocusedLists = _axeLists;
        if (_slotList[_itemFocusedIndex].Item.ItemType == ItemTypes.Boots)
            _itemFocusedLists = _bootsLists;
        if (_slotList[_itemFocusedIndex].Item.ItemType == ItemTypes.Gloves)
            _itemFocusedLists = _glovesLists;
        if (_slotList[_itemFocusedIndex].Item.ItemType == ItemTypes.Hat)
            _itemFocusedLists = _hatLists;
        if (_slotList[_itemFocusedIndex].Item.ItemType == ItemTypes.Other)
            _itemFocusedLists = _otherLists;
        if (_slotList[_itemFocusedIndex].Item.ItemType == ItemTypes.Shield)
            _itemFocusedLists = _shieldLists;
        if (_slotList[_itemFocusedIndex].Item.ItemType == ItemTypes.Staff)
            _itemFocusedLists = _staffLists;
        if (_slotList[_itemFocusedIndex].Item.ItemType == ItemTypes.Sword)
            _itemFocusedLists = _swordLists;
    }

    // Canvas > UILayerPopup > Market > BottonFrame > BuyButton
    public void Buy()
    {
        if (_inventory.IsEmptySlot() != true)
            return;

        if (_slotList[_itemFocusedIndex].Item.ItemType == ItemTypes.Ring &&
            _playerLevel < _slotList[_itemFocusedIndex].Item.RequiredLevel)
            return;

        int priceWithMarkupPercentage = Mathf.RoundToInt(_slotList[_itemFocusedIndex].Item.Price * (1 + (float)_markupPercentage / 100));
        if (_wallet.TryPayWithGold(priceWithMarkupPercentage))
        {
            _inventory.AddItem(_slotList[_itemFocusedIndex].Item);

            if (_areItemsUpdatableAfterBuying)
            {
                if (_slotList[_itemFocusedIndex].Item.ItemType != ItemTypes.Ring)
                {
                    UpdateItem();
                }
            }
            else
            {
                _marketItemList.ItemList[_itemFocusedIndex] = null;
                _slotList[_itemFocusedIndex].Item = null;
                _slotList[_itemFocusedIndex].Icon.enabled = false;
                _slotList[_itemFocusedIndex].RequiredLevelFrame.SetActive(false);
                _slotList[_itemFocusedIndex].RequiredLevelText.enabled = false;
            }
            ActivateFirstItem();
        }
    }
}
