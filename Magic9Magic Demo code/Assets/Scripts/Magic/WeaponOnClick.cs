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

    [SerializeField] private SoundManager.SoundNames _soundName;
    [SerializeField] private float _soundStartDelay;

    [SerializeField] private float _timeToSelfDestruct = 2.0f;
    private SoundManager _soundManager;

    private void OnTriggerEnter(Collider other)
    {
        EnemyTakeDamage enemy = other.gameObject.GetComponent<EnemyTakeDamage>();
        if (enemy)
        {
            enemy.TakeDamage(_magicDamages);
        }
    }

    public void Use(Transform playerTransform, Vector3 playerPosition, Vector3 targetPosition, SoundManager soundManager)
    {
        _soundManager = soundManager;
        transform.position = new Vector3(targetPosition.x, targetPosition.y + 0.1f, targetPosition.z);
        Destroy(gameObject, _timeToSelfDestruct);
        StartCoroutine(PlaySound());
    }

    private IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(_soundStartDelay);
        _soundManager.Play(_soundName.ToString());
    }
   
}
