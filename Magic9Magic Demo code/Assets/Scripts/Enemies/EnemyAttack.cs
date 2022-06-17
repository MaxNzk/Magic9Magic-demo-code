using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] EnemySettings _enemySettingsSO;
    private Animator _animator;
    private int _animIdAttack = Animator.StringToHash("attack");
    private EnemyMovement _enemyMovementScript;
    private PlayerTakeDamage _playerTakeDamageScript;
    
    private void Start()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        _animator = GetComponent<Animator>();
        _enemyMovementScript = GetComponent<EnemyMovement>();
        _playerTakeDamageScript = FindObjectOfType<PlayerTakeDamage>();
        if (_animator == null)
            Tools.LogError("Animator _animator = NULL");
        if (_enemyMovementScript == null)
            Tools.LogError("EnemyMovement _enemyMovementScript = NULL");
        if (_playerTakeDamageScript == null)
            Tools.LogError("PlayerTakeDamage _playerTakeDamageScript = NULL");
    }

    public void StartAttack()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        WaitForSeconds _waitTimeForNextAttack = new WaitForSeconds(_enemySettingsSO.AttackSpeed);
        WaitForSeconds _waitAttackMoment = new WaitForSeconds(_enemySettingsSO.WaitAttackMoment);
        while (_enemyMovementScript.IsReachedPlayer() && _playerTakeDamageScript.IsDead() == false)
        {
            _animator.SetTrigger(_animIdAttack);
            yield return _waitAttackMoment;
            _playerTakeDamageScript.TakeDamage(_enemySettingsSO.AttackValues);
            yield return _waitTimeForNextAttack;
        }
    }

}
