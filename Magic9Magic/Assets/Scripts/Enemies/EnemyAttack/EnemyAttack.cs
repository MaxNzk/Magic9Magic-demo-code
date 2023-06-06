using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    protected EnemyStatsSO _enemyStatsSO;

    protected Animator _animator;
    protected readonly int _animIdAttack = Animator.StringToHash("attack");
    
    protected EnemyMovement _enemyMovementScript;
    protected PlayerTakeDamage _playerTakeDamageScript;
    protected bool _isAttack;
    
    protected virtual void Start()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        _enemyStatsSO = Tools.GetComponentWithAssertion<EnemyStats>(gameObject).StatsSO;
        _animator = Tools.GetComponentWithAssertion<Animator>(gameObject);
        _enemyMovementScript = Tools.GetComponentWithAssertion<EnemyMovement>(gameObject);
        _playerTakeDamageScript = FindObjectOfType<PlayerTakeDamage>();
        Tools.CheckNull<PlayerTakeDamage>(_playerTakeDamageScript, nameof(_playerTakeDamageScript), gameObject);
    }

    public virtual void StartAttack()
    {
        if (_enemyStatsSO.CanAttack)
        {
            StartCoroutine("Attack");
        }
    }

    public virtual void StopAttack()
    {
        if (_isAttack && _enemyStatsSO.CanAttack)
        {
            _isAttack = false;
        }
    }

    private IEnumerator Attack()
    {
        WaitForSeconds _waitTimeForNextAttack = new WaitForSeconds(_enemyStatsSO.AttackSpeed - _enemyStatsSO.WaitTakeDamageMoment);
        WaitForSeconds _waitTakeDamageMoment = new WaitForSeconds(_enemyStatsSO.WaitTakeDamageMoment);
        _isAttack = true;

        while (_enemyMovementScript.IsReachedPlayer() && _playerTakeDamageScript.IsDead() == false)
        {
            _animator.SetTrigger(_animIdAttack);
            yield return _waitTakeDamageMoment;
            _playerTakeDamageScript.TakeDamage(_enemyStatsSO.AttackValues);
            yield return _waitTimeForNextAttack;
        }

        _isAttack = false;
    }

    public virtual bool GetAttackState()
    {
        return _isAttack;
    }

}
