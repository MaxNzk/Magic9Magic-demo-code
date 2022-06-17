using System.Collections;
using UnityEngine;

public class ManaMaker : MonoBehaviour
{
    [SerializeField] private PlayerSettings _playerSettingsSO;
    [SerializeField] private bool _isSaveManaBetweenWorlds;
    [SerializeField] private int _refillAmoung;
    [SerializeField] private float _refillSpeedInSec;
    private UIManagerPlayer _uiManager;
    private int _currentMana;
    private bool _isMaking;

    private void Start()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        if (_refillAmoung <= 0)
            Tools.LogError("_refillAmoung <= 0");
        if (_refillSpeedInSec <= 0)
            Tools.LogError("_refillSpeed <= 0");
    }

    public void Initialize(UIManagerPlayer uiManager, bool isManaLimitMode)
    {
        _uiManager = uiManager;
        SetupMana();
        StartMakeMana();
        if (isManaLimitMode == false)
            StartCoroutine(MakeMana());
    }

    private void SetupMana()
    {
        _uiManager.SetManaSliderMaxValue(_playerSettingsSO.MaxMana);
        if (_isSaveManaBetweenWorlds)
            _currentMana = _playerSettingsSO.CurrentMana;
        else
            _currentMana = _playerSettingsSO.MaxMana;
        SetCurrentMana(_currentMana);
    }

    public void StartMakeMana() => _isMaking = true;
    public void StopMakeMana() => _isMaking = false;
    
    public bool HasMana(int mana)
    {
        return _currentMana - mana >= 0 ? true : false;     
    }

    public bool GetMana(int mana)
    {
        if (_currentMana - mana >= 0)
        {
            _currentMana -= mana;
            SetCurrentMana(_currentMana);
            return true;
        }
        else
        {
            return false;
        }        
    }

    private void SetCurrentMana(int currentMana)
    {
        _playerSettingsSO.CurrentMana = currentMana;
        _uiManager.SetManaSlider(currentMana);
    }

    private IEnumerator MakeMana()
    {
        WaitForSeconds _waitForFewSeconds = new WaitForSeconds(_refillSpeedInSec);
        while (_isMaking)
        {
            _currentMana += _refillAmoung;
            _currentMana = Mathf.Clamp(_currentMana, 0, _playerSettingsSO.MaxMana);
            SetCurrentMana(_currentMana);
            yield return _waitForFewSeconds;
        }
    }

}
