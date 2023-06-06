using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour, ISkillSlot
{
    [Header("6 = Life, 7 = Death, 8 = Space")]
    [Header("3 = Wind, 4 = Metal, 5 = Lightning")]
    [Header("0 = Fire, 1 = Water, 2 = Earth")]
    [SerializeField] private int[] _defenseValues;
    [SerializeField] private string _spellName;
    [SerializeField] private Sprite _icon;
    public Sprite Icon { get => _icon; }
    [SerializeField] private int _mana;
    public int Mana { get => _mana; }
    [SerializeField] private float _cooldownTime;
    public float CooldownTime { get => _cooldownTime; }
    [SerializeField] private int _attackAnimationType;
    public int AttackAnimationType { get => _attackAnimationType; }
    [SerializeField] private int _shieldType = 1;
    private IShieldType _shieldTypeScript;
    private int[] _shieldDefenseValues;

    [SerializeField] float _duration;
    [Header("infinite if 0")]
    [SerializeField] int _durabilityTime;

    [Space(10)]
    [SerializeField] private Sound _soundName;
    [SerializeField] private float _soundStartDelay;

    private MagicPoolManager _magicPoolManager;
    private ISoundManager _soundManager;
    private CharacterStats _characterStats;
    
    private PlayerTakeDamage _playerTakeDamageScript;
    private Transform _currentPosition;

    private void OnValidate()
    {
        if (_shieldType <= 0)
        {
            _shieldType = 1;
        }
    }

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        _shieldTypeScript = Tools.GetComponentWithAssertion<IShieldType>(gameObject);
        _playerTakeDamageScript = FindObjectOfType<PlayerTakeDamage>();
        Tools.CheckNull<PlayerTakeDamage>(_playerTakeDamageScript, nameof(_playerTakeDamageScript), gameObject);
    }

    private void LateUpdate()
    {
        transform.position = _currentPosition.position;
    }

    public void Initialize(MagicPoolManager magicPoolManager, ISoundManager soundManager)
    {
        _magicPoolManager = magicPoolManager;
        _soundManager = soundManager;
        _soundManager.AddSound(_soundName.gameObject, SoundManager.ParentType.Magic.ToString());
    }

    public void Use(CharacterStats characterStats, Transform playerTransform, Vector3 playerPosition, Vector3 targetPosition)
    {
        _characterStats = characterStats;
        transform.position = playerTransform.position;
        _currentPosition = playerTransform;
        _playerTakeDamageScript.SetShield(this);
        _shieldDefenseValues = new int[_defenseValues.Length];
        for (int i = 0; i < _defenseValues.Length; i++)
        {
            _shieldDefenseValues[i] = _defenseValues[i];
        }
        StartCoroutine(PlaySound());
        if (_duration > 0)
        {
            StartCoroutine(UnsetShield(_duration));
        }
    }

    private IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(_soundStartDelay);
        _soundManager.Play(_soundName.gameObject.name);
    }

    private IEnumerator UnsetShield(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _playerTakeDamageScript.UnsetShield();
        DestroyShield();
    }

    public void UnsetShieldImmediately()
    {
        _playerTakeDamageScript.UnsetShield();
        DestroyShield();
    }

    private void DestroyShield()
    {
        StartCoroutine(ReturnToPool(0));
    }

    public int[] CalcDamagesAfterShield(int[] damages)
    {
        if (_shieldType == 3)
        {
            int[] damages2 = _shieldTypeScript.CalcDamages(damages, _shieldDefenseValues);
            CalcDamageForShieldType3(damages);
            damages = damages2;
        }
        else
        {
            damages = _shieldTypeScript.CalcDamages(damages, _defenseValues);
        }
        CalcDurabilityTime();
        return damages;
    }

    private void CalcDamageForShieldType3(int[] damages)
    {
        for (int i = 0; i < damages.Length; i++)
        {
            _shieldDefenseValues[i] -= damages[i];
            _shieldDefenseValues[i] = Mathf.Clamp(_shieldDefenseValues[i], 0, int.MaxValue);
        }
    }

    private void CalcDurabilityTime()
    {
        if (_durabilityTime == 1)
        {
            UnsetShieldImmediately();
        }
        if (_durabilityTime > 1)
        {
            if (_shieldTypeScript.IsDamaged())
            {
                _durabilityTime--;
            }
        }  
    }

    private IEnumerator ReturnToPool(float delay)
    {
        yield return new WaitForSeconds(delay);
        _magicPoolManager.ReturnObject(gameObject.name, gameObject);
    }
}
