using System.Collections;
using UnityEngine;

public class WeaponOnClick : MonoBehaviour, ISkillSlot
{
    [Header("6 = Life, 7 = Death, 8 = Space")]
    [Header("3 = Wind, 4 = Metal, 5 = Lightning")]
    [Header("0 = Fire, 1 = Water, 2 = Earth")]
    [SerializeField] private int[] _magicDamages = new int[9];
    [SerializeField] private string _spellName;
    public Sprite Icon { get => _icon; }
    [SerializeField] private int _magicColorIndex;
    public int Mana { get => _mana; }
    public float CooldownTime { get => _cooldownTime; }
    public int AttackAnimationType { get => _attackAnimationType; }
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _mana;
    [SerializeField] private float _cooldownTime;
    [SerializeField] private int _attackAnimationType;

    [SerializeField] private Sound _soundName;
    [SerializeField] private float _soundStartDelay;

    [SerializeField] private float _timeToSelfDestruct = 2.0f;
    [SerializeField] private float _transformOffsetY;

    private MagicPoolManager _magicPoolManager;
    private ISoundManager _soundManager;
    private CharacterStats _characterStats;

    private Vector3 _targetPosition;

    private void OnTriggerEnter(Collider other)
    {
        EnemyTakeDamage enemy = other.gameObject.GetComponent<EnemyTakeDamage>();
        if (enemy)
        {
            enemy.TakeDamage(GetTotalDamages());
        }
        StartCoroutine(ReturnToPool(_timeToSelfDestruct));
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
        _soundManager.AddSound(_soundName.gameObject, SoundManager.ParentType.Magic.ToString());
    }

    public void Use(CharacterStats characterStats, Transform playerTransform, Vector3 playerPosition, Vector3 targetPosition)
    {
        _characterStats = characterStats;
        _targetPosition = targetPosition;
        transform.position = new Vector3(targetPosition.x, targetPosition.y + _transformOffsetY, targetPosition.z);
        
        StartCoroutine(ReturnToPool(_timeToSelfDestruct));
        StartCoroutine(PlaySound());
    }

    private IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(_soundStartDelay);
        _soundManager.Play(_soundName.gameObject.name, _targetPosition);
    }

    private IEnumerator ReturnToPool(float delay)
    {
        yield return new WaitForSeconds(delay);
        _magicPoolManager.ReturnObject(gameObject.name, gameObject);
    }
}
