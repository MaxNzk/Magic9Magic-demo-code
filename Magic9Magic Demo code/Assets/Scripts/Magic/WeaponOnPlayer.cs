using System.Collections;
using UnityEngine;

public class WeaponOnPlayer : MonoBehaviour, ISkillSlot
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
    [SerializeField] private int _attackAnimationType;
    public float CooldownTime { get => _cooldownTime; }
    public int AttackAnimationType { get => _attackAnimationType; }

    [SerializeField] private SoundManager.SoundNames _soundName;
    [SerializeField] private float _soundStartDelay;
    [SerializeField] float _duration;

    private SoundManager _soundManager;
    private Transform _currentPosition;

    private void LateUpdate()
    {
        transform.position = _currentPosition.position;
    }

    public void Use(Transform playerTransform, Vector3 playerPosition, Vector3 targetPosition, SoundManager soundManager)
    {
        _soundManager = soundManager;
        transform.position = playerTransform.position;
        _currentPosition = playerTransform;
        if (_duration > 0)
            StartCoroutine(SetDestroy(_duration));
        StartCoroutine(PlaySound());
    }

    public void DestroyShield()
    {
        Destroy(gameObject);
    }

    private IEnumerator SetDestroy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }

    private IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(_soundStartDelay);
        _soundManager.Play(_soundName.ToString());
    }

}
