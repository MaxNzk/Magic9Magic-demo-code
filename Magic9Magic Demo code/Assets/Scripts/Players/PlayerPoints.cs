using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    [SerializeField] private PlayerSettings _playerSettingsSO;
    [SerializeField] private int _maxPoints;
    private UIManagerPlayer _uiManager;
    private int _currentPoints;
    private int _levelPoints;

    private void Start()
    {
        FindAndTestComponents();     
    }

    private void FindAndTestComponents()
    {
        if (_playerSettingsSO == null)
            Tools.LogError("PlayerSettings _playerSettingsSO = NULL");
        if (_maxPoints <= 0)
            Tools.LogError("_maxPoints <= 0");
    }

    public void Initialize(UIManagerPlayer uiManager)
    {
        _uiManager = uiManager;
        SetupPoints();
        UpdateUI();
    }

    private void SetupPoints()
    {
        _currentPoints = _playerSettingsSO.XpPoints;
        _levelPoints = _currentPoints % _maxPoints;
        _uiManager.SetPointsSliderMaxValue(_maxPoints);
    }

    private void UpdateUI()
    {
        _uiManager.SetPointsSlider(_levelPoints);
        _uiManager.SetPlayerLevel(1 + _currentPoints / _maxPoints);
    }

    public void AddPoints(int points)
    {
        _currentPoints += points;
        _playerSettingsSO.XpPoints = _currentPoints;
        _levelPoints = _currentPoints % _maxPoints;

        UpdateUI();
    }

    public int GetLevel()
    {
        int lvl = 1 + _currentPoints / _maxPoints;
        return lvl;
    }

}
