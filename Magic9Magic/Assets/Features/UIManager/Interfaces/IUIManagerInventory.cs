public interface IUIManagerInventory
{
    void ShowInventory();
    void HideInventory();
    void ShowInventoryStats();
    void UpdateItemStatsTexts(int[] attackValues, int[] defenseValues);
    void ShowInventoryItemDescription();
    void SetItemName(string itemName);
    void SetItemDescription(string itemDescription);
    void SetItemType(string itemType);
    void SetItemRequiredLevel(int itemRequiredLevel);
    void SetItemPrice(int itemPrice);
}