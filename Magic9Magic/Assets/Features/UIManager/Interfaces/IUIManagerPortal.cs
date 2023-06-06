public interface IUIManagerPortal
{
    void ShowPortal();
    void HidePortal();
    void SetWorldName(string value);
    void SetWorldDescription(string value);
    void ShowNotEnoughText();
    void SetClosePrice(string value);
    void SetMiddlePrice(string value);
    void SetFarPrice(string value);
}