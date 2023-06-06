using UnityEngine;
using TMPro;

public class UIManagerPlayer : UIManager, IUIManagerPlayer, IUIManagerWallet, IUIManagerInventory, IUIManagerMagicBook, IUIManagerJournal, IUIManagerInput, IUIManagerBloodyscreen
{
    [SerializeField] private TextMeshProUGUI _playerNameText;
    [SerializeField] private TextMeshProUGUI _playerLevelText;
    [SerializeField] private UISlider _pointsSlider;
    [SerializeField] private UISlider _healthSlider;
    [SerializeField] private UISlider _manaSlider;

    [Space(10)]
    [Header("Inventory ------------------------------------")]
    [SerializeField] private UIShowHide _inventory;
    [SerializeField] private UIShowHide _inventoryStats;
    [SerializeField] private UIStats _inventoryItemStats;

    [Space(10)]
    [SerializeField] private UIShowHide _inventoryItemDescription;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemDescriptionText;
    [SerializeField] private TextMeshProUGUI _itemTypeText;
    [SerializeField] private TextMeshProUGUI _itemRequiredLevelText;
    [SerializeField] private TextMeshProUGUI _itemPriceText;

    [Space(10)]
    [Header("Wallet ---------------------------------------")]
    [SerializeField] private TextMeshProUGUI _goldText;
    [SerializeField] private TextMeshProUGUI _soulStoneText;
    
    // For the future
    // [Space(10)]
    // [SerializeField] private UIShowHide _magicBook;
    // [SerializeField] private UIShowHide _journal;

    [Space(10)]
    [SerializeField] private BloodyScreen _bloodyScreen;

    private void Start()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckNull<TextMeshProUGUI>(_playerNameText, nameof(_playerNameText), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_playerLevelText, nameof(_playerLevelText), gameObject);
        Tools.CheckNull<UISlider>(_pointsSlider, nameof(_pointsSlider), gameObject);
        Tools.CheckNull<UISlider>(_healthSlider, nameof(_healthSlider), gameObject);
        Tools.CheckNull<UISlider>(_manaSlider, nameof(_manaSlider), gameObject);

        Tools.CheckNull<UIShowHide>(_inventory, nameof(_inventory), gameObject);
        Tools.CheckNull<UIShowHide>(_inventoryStats, nameof(_inventoryStats), gameObject);
        Tools.CheckNull<UIStats>(_inventoryItemStats, nameof(_inventoryItemStats), gameObject);

        Tools.CheckNull<UIShowHide>(_inventoryItemDescription, nameof(_inventoryItemDescription), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_itemNameText, nameof(_itemNameText), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_itemDescriptionText, nameof(_itemDescriptionText), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_itemTypeText, nameof(_itemTypeText), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_itemRequiredLevelText, nameof(_itemRequiredLevelText), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_itemPriceText, nameof(_itemPriceText), gameObject);

        Tools.CheckNull<TextMeshProUGUI>(_goldText, nameof(_goldText), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_soulStoneText, nameof(_soulStoneText), gameObject);

        // Tools.CheckNull<UIShowHide>(_magicBook, nameof(_magicBook), gameObject);
        // Tools.CheckNull<UIShowHide>(_journal, nameof(_journal), gameObject);
    }

    public void SetPlayerName(string name) => _playerNameText.text = name;
    public void SetPlayerLevel(int level) => _playerLevelText.text = level.ToString();
    public void SetPointsSlider(int value) => _pointsSlider.SetValue(value);
    public void SetPointsSliderMaxValue(int maxValue) => _pointsSlider.SetMaxValue(maxValue);
    public void SetHealthSlider(int value) => _healthSlider.SetValue(value);
    public void SetHealthSliderMaxValue(int maxValue) => _healthSlider.SetMaxValue(maxValue);
    public void SetManaSlider(int value) => _manaSlider.SetValue(value);
    public void SetManaSliderMaxValue(int maxValue) => _manaSlider.SetMaxValue(maxValue);

    public void ShowInventory() => _inventory.Show();
    public void HideInventory() => _inventory.Hide(); // Canvas > UILayerPopup > Inventory > Button_Close
    public void ShowInventoryStats() // Canvas > UILayerPopup > Inventory > StatsButton
    {
        _inventoryStats.Show();
        _inventoryItemDescription.Hide();
    }
    public void UpdateItemStatsTexts(int[] attackValues, int[] defenseValues) => _inventoryItemStats.UpdateItemStatsTexts(attackValues, defenseValues);

    public void ShowInventoryItemDescription()
    {
        _inventoryItemDescription.Show();
        _inventoryStats.Hide();
    }
    public void SetItemName(string itemName) => _itemNameText.text = itemName;
    public void SetItemDescription(string itemDescription) => _itemDescriptionText.text = itemDescription;
    public void SetItemType(string itemType) => _itemTypeText.text = itemType;
    public void SetItemRequiredLevel(int itemRequiredLevel) => _itemRequiredLevelText.text = itemRequiredLevel.ToString();
    public void SetItemPrice(int itemPrice) => _itemPriceText.text = itemPrice.ToString();

    public void SetGold(int amount) => _goldText.text = amount.ToString();
    public void SetSoulStone(int amount) => _soulStoneText.text = amount.ToString();

    public void ShowMagicBook() {}//=> _magicBook.Show();
    public void HideMagicBook() {}//=> _magicBook.Hide();
    public void ShowJournal() {}//=> _journal.Show();
    public void HideJournal() {}//=> _journal.Hide();

    public void ShowBloodyscreen() => _bloodyScreen.Show();

}
