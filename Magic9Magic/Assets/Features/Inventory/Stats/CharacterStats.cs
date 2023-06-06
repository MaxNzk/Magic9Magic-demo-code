using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private InventoryItemListSO _statsListSO;
    [SerializeField] private Button _takeOffButton;

    private PlayerSettings _playerSettingsSO;
    private IUIManagerInventory _IUIManagerInventory;
    private IUIManagerMarket _IUIManagerMarket;

    private int _equipFocusedIndex;
    public int EquipFocusedIndex
    {
        get => _equipFocusedIndex;
        set
        {
            _equipFocusedIndex = value;
            _takeOffButton.interactable = _equipFocusedIndex != ITEM_FOCUSED_NULL_INDEX;
        }
    }

    public int[] AttackValues { get; private set; }
    public int[] DefenseValues { get; private set; }

    [SerializeField, Space(10)] private List<EquipSlot> _slotEquipList = new List<EquipSlot>();
    [SerializeField] private List<Image> _slotIconList = new List<Image>();
    [SerializeField] private List<Image> _slotIconEmptyList = new List<Image>();
    [SerializeField] private List<GameObject> _slotFocusItemList = new List<GameObject>();
    [SerializeField] private List<GameObject> _slotFocusSlotList = new List<GameObject>();

    private const int MAGIC_ELEMENTS_AMOUNT = 9;
    private const int ITEM_FOCUSED_NULL_INDEX = -1;

    [Header("Inventory --------------------------------"), Space(10)]
    [SerializeField] private InventoryItemListSO InventoryScriptItemListSO;
    [field: SerializeField] public Inventory InventoryScript { get; private set; }

    [Header("Others -----------------------------------"), Space(10)]
    private Wallet _wallet;
    [field: SerializeField] public Market MarketScript { get; private set; }

    public void Initialize(PlayerSettings playerSettingsSO, ISoundManager soundManager, IUIManagerInventory iUIManagerInventory, IUIManagerMarket iUIManagerMarket)
    {
        _playerSettingsSO = playerSettingsSO;
        _IUIManagerInventory = iUIManagerInventory;
        _IUIManagerMarket = iUIManagerMarket;

        FindAndTestComponents();
        LoadItemsFromListSO();

        _IUIManagerInventory.UpdateItemStatsTexts(AttackValues, DefenseValues);

        InventoryScript.Initialize(transform, _playerSettingsSO, _wallet, soundManager, _IUIManagerInventory, InventoryScriptItemListSO, this);

        if (MarketScript != null)
            MarketScript.Initialize(InventoryScript, _wallet, _IUIManagerMarket, _playerSettingsSO);
    }

    private void FindAndTestComponents()
    { 
        _wallet = Tools.GetComponentWithAssertion<Wallet>(gameObject);

        Tools.CheckNull<Inventory>(InventoryScript, nameof(InventoryScript), gameObject);
    }

    public void DisableAllFocused()
    {
        EquipFocusedIndex = ITEM_FOCUSED_NULL_INDEX;
        SelectFocusedItem(EquipFocusedIndex);
        SelectFocusedSlot(EquipFocusedIndex);
    }

    private void LoadItemsFromListSO()
    {
        AttackValues = new int[MAGIC_ELEMENTS_AMOUNT];
        DefenseValues = new int[MAGIC_ELEMENTS_AMOUNT];

        for (int i = 0; i < _statsListSO.ItemList.Count; i++)
        {
            _slotEquipList[i].Index = i;
            _slotEquipList[i].Item = _statsListSO.ItemList[i];
            _slotEquipList[i].Initialize(this, _IUIManagerInventory, InventoryScript);

            if (_statsListSO.ItemList[i] != null)
            {     
                _slotIconEmptyList[i].enabled = false;
                _slotIconList[i].sprite = _statsListSO.ItemList[i].Icon;
                _slotIconList[i].enabled = true;

                for (int k = 0; k < MAGIC_ELEMENTS_AMOUNT; k++)
                {
                    AttackValues[k] += _statsListSO.ItemList[i].AttackValues[k];
                    DefenseValues[k] += _statsListSO.ItemList[i].DefenseValues[k];
                }
            }
            else
            {
                _slotIconList[i].sprite = null;
                _slotIconList[i].enabled = false;
                _slotIconEmptyList[i].enabled = true;
            }
        }
    }

    public InventoryItemSO Equip(InventoryItemSO item)
    {
        int index = (int) item.ItemType;

        if (_statsListSO.ItemList[index] != null)
        {
            for (int k = 0; k < MAGIC_ELEMENTS_AMOUNT; k++)
            {
                AttackValues[k] -= _statsListSO.ItemList[index].AttackValues[k];
                DefenseValues[k] -= _statsListSO.ItemList[index].DefenseValues[k];
            }
        }

        InventoryItemSO equipItem = _statsListSO.ItemList[index];
        _statsListSO.ItemList[index] = item;
        _slotEquipList[index].Item = _statsListSO.ItemList[index];
        _slotIconEmptyList[index].enabled = false;
        _slotIconList[index].sprite = _statsListSO.ItemList[index].Icon;
        _slotIconList[index].enabled = true;
        SelectFocusedItem(index);
        _slotEquipList[index].ShowItemInfo();

        for (int k = 0; k < MAGIC_ELEMENTS_AMOUNT; k++)
        {
            AttackValues[k] += _statsListSO.ItemList[index].AttackValues[k];
            DefenseValues[k] += _statsListSO.ItemList[index].DefenseValues[k];
        }

        _IUIManagerInventory.UpdateItemStatsTexts(AttackValues, DefenseValues);

        return equipItem;
    }

    // Canvas > UILayerPopup > Inventory > InventoryContainer > BottonFrame > TakeOffButton
    public void TakeOff()
    {
        if (_statsListSO.ItemList[EquipFocusedIndex] == null) return;

        InventoryScript.AddItem(_slotEquipList[EquipFocusedIndex].Item);
        InventoryScript.PlayTakeOffItemSound();

        for (int k = 0; k < MAGIC_ELEMENTS_AMOUNT; k++)
        {
            AttackValues[k] -= _statsListSO.ItemList[EquipFocusedIndex].AttackValues[k];
            DefenseValues[k] -= _statsListSO.ItemList[EquipFocusedIndex].DefenseValues[k];
        }

        _IUIManagerInventory.UpdateItemStatsTexts(AttackValues, DefenseValues);

        _statsListSO.ItemList[EquipFocusedIndex] = null;
        _slotEquipList[EquipFocusedIndex].Item = null;
        _slotIconEmptyList[EquipFocusedIndex].enabled = true;
        _slotIconList[EquipFocusedIndex].enabled = false;

        DisableAllFocused();

    }

    public void SelectFocusedItem(int activeIndex)
    {
        EquipFocusedIndex = activeIndex;
        for (int i = 0; i < _slotFocusItemList.Count; i++)
        {
            _slotFocusItemList[i].SetActive(activeIndex == i);
            _slotFocusSlotList[i].SetActive(false);
        }
    }

    public void SelectFocusedSlot(int activeIndex)
    {
        EquipFocusedIndex = ITEM_FOCUSED_NULL_INDEX;
        for (int i = 0; i < _slotFocusSlotList.Count; i++)
        {
            _slotFocusSlotList[i].SetActive(activeIndex == i);
            _slotFocusItemList[i].SetActive(false);
        }
    }

}
