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
    [SerializeField] private int _magicColorIndex;
    [SerializeField] private int _mana;
    public int Mana { get => _mana; }
    [SerializeField] private float _cooldownTime;
    [SerializeField] private int _attackAnimationType;
    public float CooldownTime { get => _cooldownTime; }
    public int AttackAnimationType { get => _attackAnimationType; }
    [SerializeField] private bool _isThroughShot;

    [SerializeField] private bool _isStartSound;
    [SerializeField] private SoundManager.SoundNames _startSoundName;
    [SerializeField] private bool _isHitSound;
    [SerializeField] private SoundManager.SoundNames _hitSoundName;
    [SerializeField] private float _soundStartDelay;
    private SoundManager _soundManager;

    [SerializeField] private float _speed = 20.0f;
    [SerializeField] private float _timeToSelfDestruct = 2.0f;
    private Vector3 _dir;

    private void Awake()
    {
        SetMagicColor(_magicColorIndex);
    }

    private void SetMagicColor(int magicColorIndex)
    {
        Renderer _renderer = GetComponent<Renderer>();
        Magic magic = FindObjectOfType<Magic>(); // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< from Factory
        if (magic == null)
            Tools.LogError("Magic magic = NULL");

        Color _color = magic.MagicColors[magicColorIndex];
        _renderer.material.color = _color;
    }

    private void Update()
    {
        transform.Translate(_dir * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyTakeDamage enemy = other.gameObject.GetComponent<EnemyTakeDamage>();
        if (enemy)
        {
            enemy.TakeDamage(_magicDamages);
            if (_isHitSound)
                _soundManager.Play(_hitSoundName.ToString());
            if (_isThroughShot == false)
                Destroy(gameObject);
        }
        if (_isThroughShot == false)
            Destroy(gameObject);
    }

    public void Use(Transform playerTransform, Vector3 playerPosition, Vector3 targetPosition, SoundManager soundManager)
    {
        _soundManager = soundManager;
        _dir = (targetPosition - playerPosition).normalized;
        transform.position = new Vector3(playerPosition.x + _dir.x, playerPosition.y, playerPosition.z + _dir.z);
        Destroy(gameObject, _timeToSelfDestruct);
        if (_isStartSound)
            StartCoroutine(PlaySound());
    }

    private IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(_soundStartDelay);
        _soundManager.Play(_startSoundName.ToString());
    }

}
