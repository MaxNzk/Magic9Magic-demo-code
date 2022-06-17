using UnityEngine;

[CreateAssetMenu(fileName = "Name", menuName = "Scriptable Objects/EnemySettings")]
public class EnemySettings : ScriptableObject
{
    [Header("6 = Life, 7 = Death, 8 = Space")]
    [Header("3 = Wind, 4 = Metal, 5 = Lightning")]
    [Header("0 = Fire, 1 = Water, 2 = Earth")]
    [SerializeField] private int[] _attackValues;
    public int[] AttackValues { get => _attackValues; }
    [SerializeField] private int[] _defenseValues;
    public int[] DefenseValues { get => _defenseValues; }
    [SerializeField] private int _xpPoints;
    public int XpPoints { get => _xpPoints; }
    [Space(10)]
    [SerializeField] private float _attackSpeed;
    public float AttackSpeed { get => _attackSpeed; }
    [SerializeField] private float _waitAttackMoment;
    public float WaitAttackMoment { get => _waitAttackMoment; }
    [Space(10)]
    [SerializeField] private int _maxHealth;
    public int MaxHealth { get => _maxHealth; }
    [SerializeField] private bool _isHealthbarShowOnAwake;
    public bool IsShowOnAwake { get => _isHealthbarShowOnAwake; }
    [SerializeField] private float _healthbarAutoHideAfterSec;
    public float HealthbarAutoHideAfterSec { get => _healthbarAutoHideAfterSec; }
    [Space(10)]
    [SerializeField] private float _speed;
    public float Speed { get => _speed; }
    [SerializeField] private float _stoppingDistance;
    public float StoppingDistance { get => _stoppingDistance; }
    [Space(10)]
    [SerializeField] private bool _isPopupText;
    public bool IsPopupText { get => _isPopupText; }
    [SerializeField] private float _popupTextOffsetY;
    public float PopupTextOffsetY { get => _popupTextOffsetY; }
}
