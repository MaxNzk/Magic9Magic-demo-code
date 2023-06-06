using System.Collections;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{
    [SerializeField] private bool _isShowBloodyscreen;
    [SerializeField] private bool _isSaveHealthBetweenWorlds;
    [SerializeField] private int minDamageForAnim;

    private int _currentHealth;
    private CharacterStats _characterStats;

    [SerializeField] private bool _isAnimator;
    private Animator _animator;
    private readonly int _animIdDamage = Animator.StringToHash("damage");
    private readonly int _animIdDead = Animator.StringToHash("dead");

    private PlayerSettings _playerSettingsSO;
    private DialogueSettingsSO _dialogueSettingsSO;
    private IFinishMission _gameManagerIFinishMission;
    private Missions.Status _missionStatus = Missions.Status.PlayerDied;
    private IUIManagerPlayer _IUIManagerPlayer;
    private IUIManagerBloodyscreen _IUIManagerBloodyscreen;
    private ISoundManager _soundManager;

    [Space(10)]
    [SerializeField] private SoundManager.SoundNamesCharacter _soundNameHit;
    [SerializeField] private SoundManager.SoundNamesUI _soundNameHealthAlarmLevel;

    private bool _isAlarmActivated;

    private bool _isDead;
    private bool _isShielded;
    private Shield _shieldScript;

    public void Initialize(PlayerSettings playerSettingsSO, DialogueSettingsSO dialogueSettingsSO, IFinishMission gameManagerIFinishMission, ISoundManager soundManager,
                           IUIManagerPlayer iUIManagerPlayer, IUIManagerBloodyscreen iUIManagerBloodyscreen)
    {
        _playerSettingsSO = playerSettingsSO;
        _dialogueSettingsSO = dialogueSettingsSO;
        _gameManagerIFinishMission = gameManagerIFinishMission;
        _soundManager = soundManager;
        _IUIManagerPlayer = iUIManagerPlayer;
        _IUIManagerBloodyscreen = iUIManagerBloodyscreen;

        FindAndTestComponents();
        SetupHealth();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckNull<PlayerSettings>(_playerSettingsSO, nameof(_playerSettingsSO), gameObject);
        if (_isAnimator)
            _animator = Tools.GetComponentInChildrenWithAssertion<Animator>(gameObject);
        
        _characterStats = GetComponent<CharacterStats>();
        if (_characterStats == null)
            _characterStats = Tools.GetComponentInChildrenWithAssertion<CharacterStats>(gameObject);
    }

    private void SetupHealth()
    {
        _IUIManagerPlayer.SetHealthSliderMaxValue(_playerSettingsSO.MaxHealth);
        if (_isSaveHealthBetweenWorlds)
        {
            _currentHealth = _playerSettingsSO.CurrentHealth;
        }
        else
        {
            _currentHealth = _playerSettingsSO.MaxHealth;
        }
        _isAlarmActivated = true;
        SetCurrentHealth();
    }

    private void CheckMannaAlarmLevel()
    {
        if (_currentHealth <= _playerSettingsSO.HealthAlarmLevel)
        {
            _soundManager.Play(_soundNameHealthAlarmLevel.ToString());
        }

        if (_isAlarmActivated == true && _currentHealth <= _playerSettingsSO.HealthAlarmLevel)
        {
            _isAlarmActivated = false;
            _soundManager.Play(_soundNameHealthAlarmLevel.ToString());
        }

        if (_isAlarmActivated == false && _currentHealth >= _playerSettingsSO.HealthAlarmRecoveryLevel)
        {
            _isAlarmActivated = true;
        }
    }

    private void SetCurrentHealth()
    {
        _playerSettingsSO.CurrentHealth = _currentHealth;
        StartCoroutine(ShowCurrentHealth(0));
    }

    public void TakeDamage(int[] damages)
    {
        if (_isShielded)
        {
            damages = _shieldScript.CalcDamagesAfterShield(damages);
        }
        int totalDamage = CalcTotalDamage(damages, _characterStats.DefenseValues);

        if (totalDamage > 0)
        {
            if (_isShowBloodyscreen)
            {
                _IUIManagerBloodyscreen.ShowBloodyscreen();
            }
            if (CheckDie(totalDamage) == false)
            {
                if (_isAnimator)
                {
                    ApplyDamageAnimation(totalDamage);
                }
                _currentHealth -= totalDamage;
                CheckMannaAlarmLevel();
                SetCurrentHealth();
                _soundManager.Play(_soundNameHit.ToString());
            }
        }
    }

    private bool CheckDie(float totalDamage)
    {
        if (_currentHealth > totalDamage)
        {
            return false;
        }

        _isDead = true;
        if (_isAnimator)
        {
            _animator.SetBool(_animIdDead, true);
        }
        _dialogueSettingsSO.IsFirstDeath = true;
        _gameManagerIFinishMission.FinishMission(_missionStatus);

        return true;
    }

    private void ApplyDamageAnimation(int damage)
    {
        if (damage >= minDamageForAnim)
        {
            _animator.SetTrigger(_animIdDamage);
        }
    }

    public void SetShield(Shield shieldScript)
    {
        if (_isShielded)
        {
            _shieldScript.UnsetShieldImmediately();
        }
        _shieldScript = shieldScript;
        _isShielded = true;
    }

    public void UnsetShield()
    {
        _isShielded = false;
    }

    private int CalcTotalDamage(int[] takenDamages, int[] defenseValues)
    {
        int totalDamage = 0;
        int tmp = 0;

        for (int i = 0; i < takenDamages.Length; i++)
        {
            tmp = defenseValues[i] - takenDamages[i];
            if (tmp < 0)
            {
                totalDamage += tmp;
            }
        }
        
        return -1 * totalDamage;
    }

    private IEnumerator ShowCurrentHealth(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _IUIManagerPlayer.SetHealthSlider(_currentHealth);
    }

    public bool IsDead()
    {
        return _isDead;
    }
}
