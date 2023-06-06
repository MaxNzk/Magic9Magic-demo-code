using System.Collections;
using UnityEngine;

public class WeaponOnShoot : MonoBehaviour, ISkillSlot
{
    [Header("6 = Life, 7 = Death, 8 = Space")]
    [Header("3 = Wind, 4 = Metal, 5 = Lightning")]
    [Header("0 = Fire, 1 = Water, 2 = Earth")]
    [SerializeField] private int[] _magicDamages = new int[9];
    [SerializeField] private string _spellName;
    [SerializeField] private Sprite _icon;
    public Sprite Icon { get => _icon; }
    [SerializeField] private int _mana;
    public int Mana { get => _mana; }
    [SerializeField] private float _cooldownTime;
    [SerializeField] private int _attackAnimationType;
    public float CooldownTime { get => _cooldownTime; }
    public int AttackAnimationType { get => _attackAnimationType; }
    [SerializeField] private bool _isThroughShot;

    [Space(10)]
    [SerializeField] private bool _isStartSound;
    [SerializeField] private Sound _startSoundName;
    [SerializeField] private bool _isHitSound;
    [SerializeField] private Sound _hitSoundName;
    [SerializeField] private float _soundStartDelay;

    private MagicPoolManager _magicPoolManager;
    private ISoundManager _soundManager;
    private CharacterStats _characterStats;

    [Space(10)]
    [SerializeField] private float _speed = 20.0f;
    [SerializeField] private float _timeToSelfDestruct = 2.0f;
    private Vector3 _dir;

    private void Update()
    {
        transform.Translate(_speed * Time.deltaTime * _dir);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyTakeDamage enemy = other.gameObject.GetComponent<EnemyTakeDamage>();
        if (enemy)
        {
            enemy.TakeDamage(GetTotalDamages());
            if (_isHitSound)
            {
                _soundManager.Play(_hitSoundName.gameObject.name, other.transform.position);
            }
        }
        if (_isThroughShot == false)
        {
            StartCoroutine(ReturnToPool(0));
        }
    }

    private int[] GetTotalDamages()
    {
        int[] total = new int[_magicDamages.Length];
        for (int i = 0; i < total.Length; i++)
        {
            total[i] = _magicDamages[i] + _characterStats.AttackValues[i];
        }
        return total;
    }

    public void Initialize(MagicPoolManager magicPoolManager, ISoundManager soundManager)
    {
        _magicPoolManager = magicPoolManager;
        _soundManager = soundManager;
        _soundManager.AddSound(_startSoundName.gameObject, SoundManager.ParentType.Magic.ToString());
        _soundManager.AddSound(_hitSoundName.gameObject, SoundManager.ParentType.Magic.ToString());
    }

    public void Use(CharacterStats characterStats, Transform playerTransform, Vector3 playerPosition, Vector3 targetPosition)
    {
        _characterStats = characterStats;
        _dir = (targetPosition - playerPosition).normalized;
        transform.position = new Vector3(playerPosition.x + _dir.x, playerPosition.y, playerPosition.z + _dir.z);
        StartCoroutine(ReturnToPool(_timeToSelfDestruct));
        if (_isStartSound)
        {
            StartCoroutine(PlaySound());
        }
    }

    private IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(_soundStartDelay);
        _soundManager.Play(_startSoundName.gameObject.name);
    }

    private IEnumerator ReturnToPool(float delay)
    {
        yield return new WaitForSeconds(delay);
        _magicPoolManager.ReturnObject(gameObject.name, gameObject);
    }
}
