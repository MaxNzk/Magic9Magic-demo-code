using System.Collections;
using UnityEngine;

public class ManaMaker : MonoBehaviour
{
    [SerializeField] private bool _isMakeManaOnInitialize = true;
    [SerializeField] private bool _isSaveManaBetweenWorlds;

    [SerializeField] private int _refillAmoung = 2;
    [SerializeField] private float _refillSpeedInSec = 1.0f;

    [Space(10)]
    [SerializeField] private SoundManager.SoundNamesUI _soundNameMannaAlarmLevel;
    [SerializeField] private SoundManager.SoundNamesUI _soundNamePickupManaCrystal;

    private PlayerSettings _playerSettingsSO;
    private ISoundManager _soundManager;
    private IUIManagerPlayer _IUIManagerPlayer;
    private bool _isManaLimitMode;
    private int _maxMissionMana;
    private int _currentMana;
    private bool _isAlarmActivated;

    public void Initialize(IManaMaker iManaMaker, PlayerSettings playerSettingsSO, ISoundManager soundManager, IUIManagerPlayer iUIManagerPlayer)
    {
        FindAndTestComponents();

        _playerSettingsSO = playerSettingsSO;
        _soundManager = soundManager;
        _IUIManagerPlayer = iUIManagerPlayer;

        _isManaLimitMode = iManaMaker.IsManaLimitMode;
        if (_isManaLimitMode)
        {
            _maxMissionMana = iManaMaker.MaxMissionMana;
        }

        SetupMana();

        if (_isMakeManaOnInitialize)
        {
            StartMakeMana();
        }
    }

    private void FindAndTestComponents()
    {
        if (_refillAmoung <= 0)
            Tools.LogError("ManaMaker: RefillAmoung <= 0");
        if (_refillSpeedInSec <= 0)
            Tools.LogError("ManaMaker: RefillSpeedInSec <= 0");
    }

    private void SetupMana()
    {
        if (_isManaLimitMode)
        {
            _IUIManagerPlayer.SetManaSliderMaxValue(_maxMissionMana);
            _currentMana = _maxMissionMana;
        }
        else
        {
            _IUIManagerPlayer.SetManaSliderMaxValue(_playerSettingsSO.MaxMana);
            if (_isSaveManaBetweenWorlds)
            {
                _currentMana = _playerSettingsSO.CurrentMana;
            }
            else
            {
                _currentMana = _playerSettingsSO.MaxMana;
            }
        }
        _isAlarmActivated = true;
        SetCurrentMana();
    }

    public void StartMakeMana() => StartCoroutine("MakeMana");
    public void StopMakeMana() => StopCoroutine("MakeMana");
    
    public bool HasMana(int mana)
    {
        return _currentMana - mana >= 0 ? true : false;     
    }

    public bool GetMana(int mana)
    {
        if (_currentMana - mana >= 0)
        {
            _currentMana -= mana;
            CheckManaAlarmLevel();
            SetCurrentMana();
            return true;
        }
        else
        {
            return false;
        }        
    }

    public bool AddMana(int mana)
    {
        if (_currentMana == _playerSettingsSO.MaxMana)
            return false;

        _currentMana += mana;
        _currentMana = Mathf.Clamp(_currentMana, 0, _playerSettingsSO.MaxMana);
        SetCurrentMana();
        _isAlarmActivated = true;
        _soundManager.Play(_soundNamePickupManaCrystal.ToString());

        return true;
    }

    private void CheckManaAlarmLevel()
    {
        if (_isAlarmActivated == true && _currentMana <= _playerSettingsSO.ManaAlarmLevel)
        {
            _isAlarmActivated = false;
            _soundManager.Play(_soundNameMannaAlarmLevel.ToString());
        }
        if (_isAlarmActivated == false && _currentMana >= _playerSettingsSO.ManaAlarmRecoveryLevel)
        {
            _isAlarmActivated = true;
        }
    }

    private void SetCurrentMana()
    {
        if (_isManaLimitMode == false)
        {
            _playerSettingsSO.CurrentMana = _currentMana;
        }
        
        _IUIManagerPlayer.SetManaSlider(_currentMana);
    }

    private IEnumerator MakeMana()
    {
        WaitForSeconds _waitForFewSeconds = new WaitForSeconds(_refillSpeedInSec);
        while (true)
        {
            if (_isManaLimitMode)
            {
                Tools.LogError("ManaMaker: MakeMana() and _isManaLimitMode at the same time");
            }
            _currentMana += _refillAmoung;
            _currentMana = Mathf.Clamp(_currentMana, 0, _playerSettingsSO.MaxMana);
            SetCurrentMana();
            yield return _waitForFewSeconds;
        }
    }

}
