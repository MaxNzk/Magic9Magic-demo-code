using System.Collections;
using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour
{
    private EnemyStatsSO _enemyStatsSO;

    [SerializeField] private float _deathAnimDuration = 2.0f;
    [HideInInspector] public bool _isReturnToPool;

    private bool _isPopupText;
    private PopupTextPool _popupTextPool;
    private int _currentHealth;
    private HealthBar _healthBar;

    private Animator _animator;
    private readonly int _animIdDamage = Animator.StringToHash("damage");
    private readonly int _animIdWalk = Animator.StringToHash("walk");
    private readonly int _animIdRun = Animator.StringToHash("run");
    private readonly int _animIdDead = Animator.StringToHash("dead");

    private PlayerPoints _playerPointsScript;
    public bool IsDead { get; private set; }
    private Collider _collider;

    private EnemyPoolManager _poolManager;
    private ILootManager _lootManager;

    private ISoundManager _soundManager;
    [SerializeField] private SoundManager.SoundNamesCharacter _soundNameDead;
    [SerializeField] private Sound _soundDeadPrefab;

    public void Initialize(bool isReturnToPool, EnemyPoolManager poolManager, ILootManager lootManager, ISoundManager soundManager, PlayerPoints playerPointsScript)
    {
        _isReturnToPool = isReturnToPool;
        _poolManager = poolManager;
        _lootManager = lootManager;
        _playerPointsScript = playerPointsScript;

        _soundManager = soundManager;
        _soundManager.AddSound(_soundDeadPrefab.gameObject, SoundManager.ParentType.Character.ToString());

        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        _collider = Tools.GetComponentWithAssertion<Collider>(gameObject);
        _animator = Tools.GetComponentWithAssertion<Animator>(gameObject);
        _healthBar = Tools.GetComponentInChildrenWithAssertion<HealthBar>(gameObject);
        
        _enemyStatsSO = GetComponent<EnemyStats>().StatsSO;
        if (_enemyStatsSO == null)
        {
            _isPopupText = false;
            Tools.LogError("EnemyStatsSO _enemyStatsSO = NULL");
        }
        else
        {
            if (_enemyStatsSO.IsPopupText)
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
    }

    private void OnEnable()
    {
        if (_enemyStatsSO != null) // block SetupHealth() from the initialization stage "Initialize()", because OnEnable() happens before initialization.
        {
            SetupHealth();
            _collider.enabled = true;
        }
    }

    private void SetupHealth()
    {
        IsDead = false;
        _currentHealth = _enemyStatsSO.MaxHealth;
        _healthBar.Initialize(_currentHealth, _enemyStatsSO.IsShowOnAwake, _enemyStatsSO.HealthbarAutoHideAfterSec);
    }

    public void TakeDamage(int[] takenDamages)
    {
        int totalDamage = CalcTotalDamage(takenDamages, _enemyStatsSO.DefenseValues);
        _currentHealth -= totalDamage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _enemyStatsSO.MaxHealth);
        
        if (_currentHealth == 0)
        {
            StartCoroutine(ShowDamage(totalDamage, isCriticalDamage: true));
            Die();
        }
        else
        {
            _animator.SetTrigger(_animIdDamage);
            StartCoroutine(ShowDamage(totalDamage, isCriticalDamage: false));
        }
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

    private void Die()
    {
        IsDead = true;
        _healthBar.SetValue(_currentHealth);

        _animator.SetBool(_animIdWalk, false);
        _animator.SetBool(_animIdRun, false);
        _animator.SetBool(_animIdDead, true);
        
        _collider.enabled = false;
        _playerPointsScript.AddPoints(_enemyStatsSO.XpPoints);
        _soundManager.Play(_soundNameDead.ToString(), transform.position);
        Vector3 lootPosition = new Vector3(transform.position.x, transform.position.y + 2.0f, transform.position.z);
        _lootManager.Get(isSenderPosition: true, lootPosition, _enemyStatsSO.Name, isSenderItemListsSOIndex: true, _enemyStatsSO.Level, 1, 1, 0f, 2.0f);
        StartCoroutine(ReturnObjToPool());
    }

    private IEnumerator ReturnObjToPool()
    {
        yield return new WaitForSeconds(_deathAnimDuration);
        if (_isReturnToPool)
        {
            _poolManager.ReturnObject(gameObject.name, gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator ShowDamage(int damage, bool isCriticalDamage)
    {
        yield return new WaitForSeconds(0.2f);
        if (_isPopupText)
        {
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + _enemyStatsSO.PopupTextOffsetY, transform.position.z);
            _popupTextPool.GetObject("PopupTextPool", newPosition, damage, _enemyStatsSO.MaxHealth, isCriticalDamage);
        }
        _healthBar.SetValue(_currentHealth);
    }
}
