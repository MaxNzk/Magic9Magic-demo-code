using UnityEngine;
using TMPro;

public class UIManagerPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerLevelText;
    [SerializeField] private UISlider _pointsSlider;
    [SerializeField] private UISlider _healthSlider;
    [SerializeField] private UISlider _manaSlider;

    // [SerializeField] private UIShowHide _magicBook;
    [SerializeField] private UIShowHide _inventory;
    // [SerializeField] private UIShowHide _journal;

    [SerializeField] private BloodyScreen _bloodyScreen;

    private void Start()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        if (_playerLevelText == null)
            Tools.LogError("TextMeshProUGUI _playerLevelText = NULL");
        if (_pointsSlider == null)
            Tools.LogError("UISlider _pointsSlider = NULL");
        if (_healthSlider == null)
            Tools.LogError("UISlider _healthSlider = NULL");
        if (_manaSlider == null)
            Tools.LogError("UISlider _manaSlider = NULL");

        // if (_magicBook == null)
        //     Tools.LogError("UIShowHide _magicBook = NULL");
        if (_inventory == null)
            Tools.LogError("UIShowHide _inventory = NULL");
        // if (_journal == null)
        //     Tools.LogError("UIShowHide _journal = NULL");
    }

    public void SetPlayerLevel(int level) => _playerLevelText.text = level.ToString();
    public void SetPointsSlider(int value) => _pointsSlider.SetValue(value);
    public void SetPointsSliderMaxValue(int maxValue) => _pointsSlider.SetMaxValue(maxValue);
    public void SetHealthSlider(int value) => _healthSlider.SetValue(value);
    public void SetHealthSliderMaxValue(int maxValue) => _healthSlider.SetMaxValue(maxValue);
    public void SetManaSlider(int value) => _manaSlider.SetValue(value);
    public void SetManaSliderMaxValue(int maxValue) => _manaSlider.SetMaxValue(maxValue);

    public void ShowMagicBook() {}//=> _magicBook.Show();
    public void HideMagicBook() {}//=> _magicBook.Hide();
    public void ShowInventory() => _inventory.Show();
    public void HideInventory() => _inventory.Hide(); // Canvas > UILayerPopup > Inventory > Button_Close
    public void ShowJournal() {}//=> _journal.Show();
    public void HideJournal() {}//=> _journal.Hide();

    public void ShowBloodyscreen() => _bloodyScreen.Show();

}
