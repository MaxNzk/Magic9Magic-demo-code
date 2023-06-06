using System;
using UnityEngine;

public class PlayerPoints : MonoBehaviour
{
    public static event Action<int> OnPlayerLevelChanged;

    private PlayerSettings _playerSettingsSO;
    private IUIManagerPlayer _IUIManagerPlayer;
    private IAnalytics _analytics;

    private int _currentPoints;
    private int _levelPoints;
    private int _currentLevel;

    public void Initialize(PlayerSettings playerSettingsSO, IUIManagerPlayer iUIManagerPlayer, IAnalytics analytics)
    {
        _playerSettingsSO = playerSettingsSO;
        _IUIManagerPlayer = iUIManagerPlayer;
        _analytics = analytics;

        FindAndTestComponents();
        SetupPoints();
        UpdateUI();
    }

    private void FindAndTestComponents()
    {
        if (_playerSettingsSO.MaxLevelPoints <= 0)
            Tools.LogError("PlayerPoints: _playerSettingsSO.MaxLevelPoints <= 0");
    }

    private void SetupPoints()
    {
        _currentPoints = _playerSettingsSO.XpPoints;
        _levelPoints = _currentPoints % _playerSettingsSO.MaxLevelPoints;
        _currentLevel = GetLevel();
        _IUIManagerPlayer.SetPointsSliderMaxValue(_playerSettingsSO.MaxLevelPoints);
    }

    private void UpdateUI()
    {
        _IUIManagerPlayer.SetPointsSlider(_levelPoints);
        _IUIManagerPlayer.SetPlayerLevel(1 + _currentPoints / _playerSettingsSO.MaxLevelPoints);
    }

    public void AddPoints(int points)
    {
        _currentPoints += points;
        _playerSettingsSO.XpPoints = _currentPoints;
        _levelPoints = _currentPoints % _playerSettingsSO.MaxLevelPoints;
        int level = GetLevel();
        if (_currentLevel != level)
        {
            _currentLevel = GetLevel();
            _playerSettingsSO.Level = _currentLevel;
            _analytics.SendNewPlayerLevel(_currentLevel);
            _playerSettingsSO.IsLevelChanged = true;
            OnPlayerLevelChanged?.Invoke(_currentLevel);
        }
        UpdateUI();
    }

    public int GetLevel()
    {
        int level = 1 + _currentPoints / _playerSettingsSO.MaxLevelPoints;
        return level;
    }

}
