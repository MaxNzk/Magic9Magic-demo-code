public interface IUIManagerPlayer
{
    void SetPlayerName(string name);
    void SetPlayerLevel(int level);
    void SetPointsSlider(int value);
    void SetPointsSliderMaxValue(int maxValue);
    void SetHealthSlider(int value);
    void SetHealthSliderMaxValue(int maxValue);
    void SetManaSlider(int value);
    void SetManaSliderMaxValue(int maxValue);
}