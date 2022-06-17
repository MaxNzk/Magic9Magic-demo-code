using System.Collections;
using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour
{
    [SerializeField] private EnemySettings _enemySettingsSO;
    private bool _isPopupText;
    private PopupTextPool _popupTextPool;
    private int _currentHealth;
    private HealthBar _healthBar;
    private Animator _animator;
    private int _animIdDamage = Animator.StringToHash("damage");
    private int _animIdWalk = Animator.StringToHash("walk");
    private int _animIdRun = Animator.StringToHash("run");
    private int _animIdDead = Animator.StringToHash("dead");

    private PlayerPoints _playerPointsScript;
    public bool IsDead { get; private set; }
    private ScriptableObjectPoolManager _poolManager;
    private Collider _collider;

    private void Awake()
    {
        _poolManager = FindObjectOfType<ScriptableObjectPoolManager>();
        FindAndTestComponents();
        SetupHealth();
    }

    private void OnEnable()
    {
        SetupHealth();
        _collider.enabled = true;
    }

    private void FindAndTestComponents()
    {
        _collider = GetComponent<Collider>();
        _playerPointsScript = FindObjectOfType<PlayerPoints>();
        _animator = GetComponent<Animator>();
        _healthBar = GetComponentInChildren<HealthBar>();
        
        if (_enemySettingsSO == null)
        {
            _isPopupText = false;
            Tools.LogError("EnemySettings _enemySettingsSO = NULL");
        }
        else
        {
            if (_enemySettingsSO.IsPopupText)
            {
                _isPopupText = true;
                _popupTextPool = FindObjectOfType<PopupTextPool>();
                if (_popupTextPool == null)
                {
                    _isPopupText = false;
                    Tools.LogError("PopupTextPool _popupTextPool = NULL");
                }
            }
        }
        if (_poolManager == null)
            Tools.LogError("ScriptableObjectPoolManager _poolManager = NULL");
        if (_playerPointsScript == null)
            Tools.LogError("PlayerPoints _playerPointsScript = NULL");
        if (_animator == null)
            Tools.LogError("Animator _animator = NULL");
        if (_healthBar == null)
            Tools.LogError("HealthBar _healthBar = NULL");
    }

    private void SetupHealth()
    {
        IsDead = false;
        _currentHealth = _enemySettingsSO.MaxHealth;
        _healthBar.Initialize(_currentHealth, _enemySettingsSO.IsShowOnAwake, _enemySettingsSO.HealthbarAutoHideAfterSec);
    }

    public void TakeDamage(int[] takenDamages)
    {
        int totalDamage = CalcTotalDamage(takenDamages, _enemySettingsSO.DefenseValues);
        _currentHealth -= totalDamage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _enemySettingsSO.MaxHealth);
        
        if (_currentHealth == 0)
        {
            StartCoroutine(ShowDamage(totalDamage));
            Die();
        }
        else
        {
            _animator.SetTrigger(_animIdDamage);
            StartCoroutine(ShowDamage(totalDamage));
        }
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

    private void Die()
    {
        IsDead = true;
        _healthBar.SetValue(_currentHealth);
        _animator.SetBool(_animIdWalk, false);
        _animator.SetBool(_animIdRun, false);
        _animator.SetBool(_animIdDead, true);
        _collider.enabled = false;
        _playerPointsScript.AddPoints(_enemySettingsSO.XpPoints);
        StartCoroutine(ReturnObjToPool());
    }

    private IEnumerator ReturnObjToPool()
    {
        yield return new WaitForSeconds(2.0f);
        _poolManager.ReturnObject(gameObject.name, gameObject);
    }

    private IEnumerator ShowDamage(int damage)
    {
        yield return new WaitForSeconds(0.2f);
        if (_isPopupText)
        {
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + _enemySettingsSO.PopupTextOffsetY, transform.position.z);
            _popupTextPool.GetObject("PopupTextPool", newPosition, damage);
        }
        _healthBar.SetValue(_currentHealth);
    }
}
