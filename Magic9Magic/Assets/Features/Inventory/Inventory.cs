using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private bool IsSellButtonActive;

    private List<InventorySlot> _slotList = new List<InventorySlot>();
    private InventoryItemListSO _inventoryItemListSO;

    public InventorySlot FocusedCurrentInventorySlot { get; set; }
    private int _itemFocusedIndex;
    public int ItemFocusedIndex
    {
        get => _itemFocusedIndex;
        set
        {
            _itemFocusedIndex = value;
            _sellButton.interactable = _itemFocusedIndex != ITEM_FOCUSED_NULL_INDEX;
            _equipButton.interactable = _itemFocusedIndex != ITEM_FOCUSED_NULL_INDEX;
            _dropButton.interactable = _itemFocusedIndex != ITEM_FOCUSED_NULL_INDEX;

            if (_itemFocusedIndex != ITEM_FOCUSED_NULL_INDEX)
            {
                ItemTypes currentItemType = _slotList[ItemFocusedIndex].Item.ItemType;
                if (currentItemType == ItemTypes.Ring || currentItemType == ItemTypes.Crystal || currentItemType == ItemTypes.EnemyPart || currentItemType == ItemTypes.Plant)
                {
                    _equipButton.interactable = false;
                }
                if (_playerSettings.Level < _inventoryItemListSO.ItemList[ItemFocusedIndex].RequiredLevel)
                {
                    _equipButton.interactable = false;
                }
            }
        }
    }

    #if UNITY_EDITOR
        [field: SerializeField, ReadOnly] public int EmptySlotAmount { get; private set; }
    #else
        [field: SerializeField] public int EmptySlotAmount { get; private set; }
    #endif

    private int _slotAmount;

    [Space(10)]
    [Header("Sounds -------------------------------------")]
    [SerializeField] private SoundManager.SoundNamesUI _soundNameRingOpening;
    [SerializeField] private SoundManager.SoundNamesUI _soundNameRingClosing;
    [SerializeField] private SoundManager.SoundNamesUI _soundNamePickupItem;
    [SerializeField] private SoundManager.SoundNamesUI _soundNameEquipItem;
    [SerializeField] private SoundManager.SoundNamesUI _soundNameTakeOffItem;
    [SerializeField] private SoundManager.SoundNamesUI _soundNameBeginDragItem;
    [SerializeField] private SoundManager.SoundNamesUI _soundNameEndDragItem;
    [SerializeField] private SoundManager.SoundNamesUI _soundNameSellItem;
    [SerializeField] private SoundManager.SoundNamesUI _soundNameDropItem;

    [Space(10)]
    [Header("UI -----------------------------------------")]
    [SerializeField] private RectTransform _slotsContainer;
    [SerializeField] private GameObject _inventorySlotPrefab;
    [SerializeField] private Button _sellButton;
    [SerializeField] private Button _equipButton;
    [SerializeField] private Button _dropButton;
    public RectTransform DraggingIcon;

    [Space(10)]
    [Header("Drop Item Settings -------------------------")]
    [SerializeField] float _dropItemRadius = 2.5f;
    [SerializeField] float _dropItemOffsetY = 1.0f;

    private Transform _player;
    private PlayerSettings _playerSettings;
    private Wallet _wallet;
    private ISoundManager _soundManager;
    private IUIManagerInventory _IUIManagerInventory;
    private CharacterStats _characterStats;

    private const int ITEM_FOCUSED_NULL_INDEX = -1;

    public void Initialize(Transform player, PlayerSettings playerSettings, Wallet wallet, ISoundManager soundManager, IUIManagerInventory iUIManagerInventory, InventoryItemListSO inventoryItemListSO, CharacterStats characterStats)
    {
        _player = player;
        _playerSettings = playerSettings;
        _wallet = wallet;
        _soundManager = soundManager;
        _IUIManagerInventory = iUIManagerInventory;
        _inventoryItemListSO = inventoryItemListSO;
        _characterStats = characterStats;

        SetupSettings();
        CreateSlots();
        LoadItemsFromListSO();
        
        _sellButton.gameObject.SetActive(IsSellButtonActive);
        DraggingIcon.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        UnfocusCurrentItem();

        _characterStats.DisableAllFocused();
        _IUIManagerInventory.ShowInventoryStats();
        _soundManager.Play(_soundNameRingOpening.ToString());
    }

    private void OnDisable()
    {
        _soundManager.Play(_soundNameRingClosing.ToString());
    }

    private void SetupSettings()
    {
        _slotAmount = _playerSettings.RingSlotAmount;
        EmptySlotAmount = _slotAmount;
    }

    private void CreateSlots()
    {
        for (int i = 0; i < _slotAmount; i++)
        {
            GameObject item = Instantiate(_inventorySlotPrefab);
            item.transform.SetParent(_slotsContainer, false);
            InventorySlot itemSlotScript = item.GetComponent<InventorySlot>();
            itemSlotScript.ItemListIndex = i;
            itemSlotScript.IsDragDrop = true;
            itemSlotScript.Initialize(this, _IUIManagerInventory, _characterStats);
            _slotList.Add(itemSlotScript);
        }
    }

    private void LoadItemsFromListSO()
    {
        for (int i = 0; i < _inventoryItemListSO.ItemList.Count; i++)
        {
            if (i == _slotAmount)
                break;

            if (_inventoryItemListSO.ItemList[i] != null)
            {
                _slotList[i].Item = _inventoryItemListSO.ItemList[i];
                _slotList[i].Icon.sprite = _inventoryItemListSO.ItemList[i].Icon;
                _slotList[i].Icon.enabled = true;
                _slotList[i].RequiredLevelFrame.SetActive(true);
                _slotList[i].RequiredLevelText.text = _inventoryItemListSO.ItemList[i].RequiredLevel.ToString();
                _slotList[i].RequiredLevelText.enabled = true;
                EmptySlotAmount--;
            }
        }
    }

    public void AddItem(InventoryItemSO item)
    {
        if (IsItemRing(item))
            return;

        bool wasEmpty = false;
        for (int i = 0; i < _slotAmount; i++)
        {
            if (_slotList[i].Item == null)
            {
                _inventoryItemListSO.ItemList[i] = item;
                _slotList[i].Item = item;
                _slotList[i].Icon.sprite = item.Icon;
                _slotList[i].Icon.enabled = true;
                _slotList[i].RequiredLevelFrame.SetActive(true);
                _slotList[i].RequiredLevelText.text = item.RequiredLevel.ToString();
                _slotList[i].RequiredLevelText.enabled = true;

                ItemFocusedIndex = i;
                FocusedCurrentInventorySlot = _slotList[ItemFocusedIndex];
                EmptySlotAmount--;

                _slotList[ItemFocusedIndex].Focus.SetActive(true);

                wasEmpty = true;
                break;
            }
        }
        if (wasEmpty == false)
        {
            Vector3 pos = _player.position + Tools.GetRandomOnUnitCircle(_dropItemRadius) + new Vector3(0, _dropItemOffsetY, 0); 
            Instantiate(item.ItemPrefab, pos, Quaternion.identity);
        }
    }

    public void PlayPickupItemSound() => _soundManager.Play(_soundNamePickupItem.ToString());
    public void PlayBeginDragItemSound() => _soundManager.Play(_soundNameBeginDragItem.ToString());
    public void PlayEndDragItemSound() => _soundManager.Play(_soundNameEndDragItem.ToString());
    public void PlayTakeOffItemSound() => _soundManager.Play(_soundNameTakeOffItem.ToString());

    private bool IsItemRing(InventoryItemSO item)
    {
        if (item.ItemType == ItemTypes.Ring)
        {
            if (item.AttackValues[0] <= _playerSettings.RingSlotAmount)
                return true;
            
            _playerSettings.RingSlotAmount = item.AttackValues[0];
            int oldSlotAmount = _slotAmount;
            _slotAmount = _playerSettings.RingSlotAmount;

            for (int i = oldSlotAmount; i < _slotAmount; i++)
            {
                GameObject itemSlot = Instantiate(_inventorySlotPrefab);
                itemSlot.transform.SetParent(_slotsContainer, false);
                InventorySlot itemSlotScript = itemSlot.GetComponent<InventorySlot>();
                itemSlotScript.ItemListIndex = i;
                itemSlotScript.IsDragDrop = true;
                itemSlotScript.Initialize(this, _IUIManagerInventory, _characterStats);
                _slotList.Add(itemSlotScript);
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    // Canvas > UILayerPopup > Inventory > InventoryContainer > BottonFrame > DropButton
    public void DropItem()
    {
        if (FocusedCurrentInventorySlot == null)
            return;

        Vector3 pos = _player.position + Tools.GetRandomOnUnitCircle(_dropItemRadius) + new Vector3(0, _dropItemOffsetY, 0); 
        Instantiate(_inventoryItemListSO.ItemList[ItemFocusedIndex].ItemPrefab, pos, Quaternion.identity);

        RemoveItem(ItemFocusedIndex);
        _characterStats.DisableAllFocused();
        ShowInventoryStats();
        _soundManager.Play(_soundNameDropItem.ToString());
    }

    // Canvas > UILayerPopup > Inventory > InventoryContainer > BottonFrame > SellButton
    public void SellItem()
    {
        if (FocusedCurrentInventorySlot == null)
            return;

        _wallet.GetMoney(_inventoryItemListSO.ItemList[ItemFocusedIndex].Price);

        RemoveItem(ItemFocusedIndex);
        _characterStats.DisableAllFocused();
        ShowInventoryStats();
        _soundManager.Play(_soundNameSellItem.ToString());
    }

    // Canvas > UILayerPopup > Inventory > InventoryContainer > BottonFrame > EquipButton
    public void EquipItem()
    {
        if (FocusedCurrentInventorySlot == null)
            return;

        _soundManager.Play(_soundNameEquipItem.ToString());

        InventoryItemSO itemFromEquip = _characterStats.Equip(_inventoryItemListSO.ItemList[ItemFocusedIndex]);
        if (itemFromEquip == null)
        {
            RemoveItem(ItemFocusedIndex);
            return;
        }

        _inventoryItemListSO.ItemList[ItemFocusedIndex] = itemFromEquip;

        _slotList[ItemFocusedIndex].Item = itemFromEquip;
        _slotList[ItemFocusedIndex].Icon.sprite = itemFromEquip.Icon;
        _slotList[ItemFocusedIndex].Icon.enabled = true;
        _slotList[ItemFocusedIndex].RequiredLevelFrame.SetActive(true);
        _slotList[ItemFocusedIndex].RequiredLevelText.text = itemFromEquip.RequiredLevel.ToString();
        _slotList[ItemFocusedIndex].RequiredLevelText.enabled = true;
    }

    public void SwapItems(int dragIndex, int dropIndex)
    {
        _inventoryItemListSO.ItemList.Swap(dragIndex, dropIndex);

        _slotList[dragIndex].Item = _inventoryItemListSO.ItemList[dragIndex];
        _slotList[dropIndex].Item = _inventoryItemListSO.ItemList[dropIndex];

        if (_slotList[dragIndex].Item != null)
        {
            _slotList[dragIndex].Icon.sprite = _slotList[dragIndex].Item.Icon;
            _slotList[dragIndex].Icon.enabled = true;
            _slotList[dragIndex].RequiredLevelFrame.SetActive(true);
            _slotList[dragIndex].RequiredLevelText.text = _slotList[dragIndex].Item.RequiredLevel.ToString();
            _slotList[dragIndex].RequiredLevelText.enabled = true;
        }
        else
        {
            _slotList[dragIndex].Icon.sprite = null;
            _slotList[dragIndex].Icon.enabled = false;
            _slotList[dragIndex].RequiredLevelFrame.SetActive(false);
            _slotList[dragIndex].RequiredLevelText.enabled = false;
        }
        
        if (_slotList[dropIndex].Item != null)
        {
            _slotList[dropIndex].Icon.sprite = _slotList[dropIndex].Item.Icon;
            _slotList[dropIndex].Icon.enabled = true;
            _slotList[dropIndex].RequiredLevelFrame.SetActive(true);
            _slotList[dropIndex].RequiredLevelText.text = _slotList[dropIndex].Item.RequiredLevel.ToString();
            _slotList[dropIndex].RequiredLevelText.enabled = true;
        }
        else
        {
            _slotList[dropIndex].Icon.sprite = null;
            _slotList[dropIndex].Icon.enabled = false;
            _slotList[dropIndex].RequiredLevelFrame.SetActive(false);
            _slotList[dropIndex].RequiredLevelText.enabled = false;
        }

        _slotList[ItemFocusedIndex].Focus.SetActive(false);
        ItemFocusedIndex = dropIndex;
        _slotList[ItemFocusedIndex].Focus.SetActive(true);

        FocusedCurrentInventorySlot = _slotList[dropIndex];
    }

    // Canvas > UILayerPopup > Inventory > InventoryContainer > BottonFrame > StatsButton
    public void UnfocusCurrentItem()
    {
        if (ItemFocusedIndex != ITEM_FOCUSED_NULL_INDEX)
        {
            _slotList[ItemFocusedIndex].Focus.SetActive(false);
            ItemFocusedIndex = ITEM_FOCUSED_NULL_INDEX;
        }
    }

    public bool IsEmptySlot() => EmptySlotAmount > 0;

    private void RemoveItem(int index)
    {
        _inventoryItemListSO.ItemList[index] = null;
        _slotList[ItemFocusedIndex].Focus.SetActive(false);
        _slotList[index].Icon.enabled = false;
        _slotList[index].RequiredLevelFrame.SetActive(false);
        _slotList[index].RequiredLevelText.enabled = false;
        _slotList[index].Item = null;

        EmptySlotAmount++;
        FocusedCurrentInventorySlot = null;
        ItemFocusedIndex = ITEM_FOCUSED_NULL_INDEX;
    }

    private void ShowInventoryStats()
    {
        _IUIManagerInventory.ShowInventoryStats();
    }

}
