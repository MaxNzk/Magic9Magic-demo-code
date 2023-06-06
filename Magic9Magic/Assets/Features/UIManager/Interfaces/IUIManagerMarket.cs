public interface IUIManagerMarket
{
    void ShowMarket();
    void HideMarket();
    void SetItemName(string itemName);
    void SetItemDescription(string itemDescription);
    void SetItemType(string itemType);
    void SetItemRequiredLevel(int itemRequiredLevel);
    void SetItemPrice(int itemPrice);
}