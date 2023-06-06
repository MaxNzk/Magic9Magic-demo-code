using UnityEngine;
using TMPro;

public class UIManagerCamp : UIManager, IUIManagerPortal, IUIManagerMarket, IUIManagerMenu, IUIManagerTutorialInput
{
    [SerializeField] private UIShowHide _menu;
    [SerializeField] private UIShowHide _tutorialInput;

    [Space(10)]
    [Header("Portal ---------------------------------------")]
    [SerializeField] private UIShowHide _portal;
    [SerializeField] private TextMeshProUGUI _worldNameText;
    [SerializeField] private TextMeshProUGUI _worldDescriptionText;
    [SerializeField] private UIShowForAWhile _notEnoughText;
    [SerializeField] private TextMeshProUGUI _closePriceText;
    [SerializeField] private TextMeshProUGUI _middlePriceText;
    [SerializeField] private TextMeshProUGUI _farPriceText;

    [Space(10)]
    [Header("Market ---------------------------------------")]
    [SerializeField] private UIShowHide _market;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemDescriptionText;
    [SerializeField] private TextMeshProUGUI _itemTypeText;
    [SerializeField] private TextMeshProUGUI _itemRequiredLevelText;
    [SerializeField] private TextMeshProUGUI _itemPriceText;

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckNull<UIShowHide>(_menu, nameof(_menu), gameObject);
        Tools.CheckNull<UIShowHide>(_tutorialInput, nameof(_tutorialInput), gameObject);

        Tools.CheckNull<UIShowHide>(_portal, nameof(_portal), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_worldNameText, nameof(_worldNameText), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_worldDescriptionText, nameof(_worldDescriptionText), gameObject);
        Tools.CheckNull<UIShowForAWhile>(_notEnoughText, nameof(_notEnoughText), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_closePriceText, nameof(_closePriceText), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_middlePriceText, nameof(_middlePriceText), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_farPriceText, nameof(_farPriceText), gameObject);

        Tools.CheckNull<UIShowHide>(_market, nameof(_market), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_itemNameText, nameof(_itemNameText), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_itemDescriptionText, nameof(_itemDescriptionText), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_itemTypeText, nameof(_itemTypeText), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_itemRequiredLevelText, nameof(_itemRequiredLevelText), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_itemPriceText, nameof(_itemPriceText), gameObject);
    }

    public void ShowMenu() => _menu.Show();
    public void HideMenu() => _menu.Hide();
    
    public void ShowTutorialInput() => _tutorialInput.Show();
    public void HideTutorialInput() => _tutorialInput.Hide();

    public void ShowPortal() => _portal.Show();
    public void HidePortal() => _portal.Hide();
    public void SetWorldName(string value) => _worldNameText.text = value;
    public void SetWorldDescription(string value) => _worldDescriptionText.text = value;
    public void ShowNotEnoughText() => _notEnoughText.Show();
    public void SetClosePrice(string value) => _closePriceText.text = value;
    public void SetMiddlePrice(string value) => _middlePriceText.text = value;
    public void SetFarPrice(string value) => _farPriceText.text = value;

    public void ShowMarket() => _market.Show();
    public void HideMarket() => _market.Hide();
    public void SetItemName(string itemName) => _itemNameText.text = itemName;
    public void SetItemDescription(string itemDescription) => _itemDescriptionText.text = itemDescription;
    public void SetItemType(string itemType) => _itemTypeText.text = itemType;
    public void SetItemRequiredLevel(int itemRequiredLevel) => _itemRequiredLevelText.text = itemRequiredLevel.ToString();
    public void SetItemPrice(int itemPrice) => _itemPriceText.text = itemPrice.ToString();

}
