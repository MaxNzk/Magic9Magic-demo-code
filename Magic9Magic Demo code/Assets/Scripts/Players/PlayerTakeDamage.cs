using System.Collections;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{
    [SerializeField] private PlayerSettings _playerSettingsSO;
    [SerializeField] private bool _isShowBloodyscreen;
    [SerializeField] private bool _isSaveHealthBetweenWorlds;
    [SerializeField] private int minDamageForAnim;
    private int _currentHealth;

    [SerializeField] private bool _isAnimator;
    private Animator _animator;
    private int _animIdDamage = Animator.StringToHash("damage");
    private int _animIdDead = Animator.StringToHash("dead");

    private IFinishMission _gameManagerIFinishMission;
    private Missions.Status _missionStatus = Missions.Status.PlayerDied;
    private UIManagerPlayer _uiManagerPlayer;
    private PlayerPoints _playerPoints;
    private bool _isDead;
    private bool _isShielded;
    private Shield _shieldScript;

    private void Start()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        _playerPoints = GetComponent<PlayerPoints>();
        if (_isAnimator)
        {
            _animator = GetComponentInChildren<Animator>();
            if (_animator == null)
                Tools.LogError("Animator _animator = NULL");
        }
        if (_playerSettingsSO == null)
            Tools.LogError("PlayerSettings _playerSettingsSO = NULL");
        if (_playerPoints == null)
            Tools.LogError("PlayerPoints _playerPoints = NULL");
    }

    public void Initialize(IFinishMission gameManagerIFinishMission, UIManagerPlayer uiManagerPlayer)
    {
        _gameManagerIFinishMission = gameManagerIFinishMission;
        _uiManagerPlayer = uiManagerPlayer;
        SetupHealth();
    }

    private void SetupHealth()
    {
        _uiManagerPlayer.SetHealthSliderMaxValue(_playerSettingsSO.MaxHealth);
        if (_isSaveHealthBetweenWorlds)
            _currentHealth = _playerSettingsSO.CurrentHealth;
        else
            _currentHealth = _playerSettingsSO.MaxHealth;
        SetCurrentHealth(_currentHealth);
    }

    private void SetCurrentHealth(int currentHealth)
    {
        _playerSettingsSO.CurrentHealth = currentHealth;
        StartCoroutine(ShowCurrentHealth(0));
    }

    public void TakeDamage(int[] damages)
    {
        if (_isShielded)
            damages = _shieldScript.CalcDamagesAfterShield(damages);
        int totalDamage = CalcTotalDamage(damages, _playerSettingsSO.DefenseValues);

        if (totalDamage > 0)
        {
            if (_isShowBloodyscreen)
                _uiManagerPlayer.ShowBloodyscreen();
            if (CheckDie(totalDamage) == false)
            {
                if (_isAnimator)
                    ApplyDamageAnimation(totalDamage);
                _currentHealth -= totalDamage;
                SetCurrentHealth(_currentHealth);
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
            _animator.SetBool(_animIdDead, true); 
        _gameManagerIFinishMission.FinishMission(_missionStatus);
        return true;
    }

    private void ApplyDamageAnimation(int damage)
    {
        if (damage >= minDamageForAnim)
            _animator.SetTrigger(_animIdDamage);
    }

    public void SetShield(Shield shieldScript)
    {
        if (_isShielded)
            _shieldScript.UnsetShieldImmediately();
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
            if (tmp < 0) totalDamage += tmp;
        }
        return -1 * totalDamage;
    }

    private IEnumerator ShowCurrentHealth(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _uiManagerPlayer.SetHealthSlider(_currentHealth);
    }

    public bool IsDead()
    {
        return _isDead;
    }
}
