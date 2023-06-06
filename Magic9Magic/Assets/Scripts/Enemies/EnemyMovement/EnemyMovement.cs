using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    protected EnemyStatsSO _enemyStatsSO;
    
    private Transform _transform;
    private NavMeshAgent _agent;

    private Animator _animator;
    private readonly int _animIdWalk = Animator.StringToHash("walk");

    private Transform _player;
    private PlayerTakeDamage _playerTakeDamageScript;
    private bool _isReachedPlayer;
    private bool _isStopped;
    private EnemyAttack _enemyAttackScript;
    private EnemyTakeDamage _enemyTakeDamageScript;
    private float _speedModifier = 1.0f;
    private bool _isInitialized;
    
    public void Initialize(Transform player)
    {
        if (_isInitialized)
            return;

        _player = player;

        GameManagerWorld1.OnFinishMission += Stop;
        GameManagerWorld2.OnFinishMission += Stop;

        FindAndTestComponents();
        SetupAgent();

        _isInitialized = true;     
    }

    protected virtual Vector3 GetDestination()
    {
        if (_player == null)
        {
            Tools.LogError("EnemyMoveToTarget: GetDestination() return Vector3.zero");
            return Vector3.zero;
        }
        return _player.position;
    }

    protected virtual Transform LookAtTarget()
    {
        return _player;
    }

    private void OnEnable()
    {
        _isStopped = false;
        _isReachedPlayer = false;
    }

    private void OnDestroy()
    {
        GameManagerWorld1.OnFinishMission -= Stop;
        GameManagerWorld2.OnFinishMission -= Stop;
    }

    protected virtual void FindAndTestComponents()
    {
        _enemyStatsSO = Tools.GetComponentWithAssertion<EnemyStats>(gameObject).StatsSO;
        _transform = Tools.GetComponentWithAssertion<Transform>(gameObject);
        
        _enemyAttackScript = Tools.GetComponentWithAssertion<EnemyAttack>(gameObject);
        _enemyTakeDamageScript = Tools.GetComponentWithAssertion<EnemyTakeDamage>(gameObject);

        _agent = Tools.GetComponentWithAssertion<NavMeshAgent>(gameObject);
        _animator = Tools.GetComponentWithAssertion<Animator>(gameObject);

        _playerTakeDamageScript = _player.GetComponent<PlayerTakeDamage>();
        Tools.CheckNull<Transform>(_player, nameof(_player), gameObject);
        Tools.CheckNull<PlayerTakeDamage>(_playerTakeDamageScript, nameof(_playerTakeDamageScript), gameObject);
    }

    private void SetupAgent()
    {
        _agent.speed = _enemyStatsSO.Speed;
        _agent.stoppingDistance = _enemyStatsSO.StoppingDistance;
        _isReachedPlayer = true; // anti-first call: if (_isReachedPlayer == false)
    }

    private void Update()
    {
        if (_isInitialized == false)
            return;
        
        if (CanMove() == false)
            return;

        SetDestination();
        Move();
    }

    private bool CanMove()
    {
        if (_enemyStatsSO.CanMove == false)
            return false;
        
        if (_isStopped || _enemyTakeDamageScript.IsDead || _playerTakeDamageScript.IsDead())
        {
            _agent.isStopped = true;
            return false;
        }
        return true;
    }

    private void SetDestination()
    {
        _agent.SetDestination(GetDestination());
    }

    private void Move()
    {
        if (_agent.pathPending)
            return;

        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (_enemyAttackScript.GetAttackState() == false && _isReachedPlayer == false)
            {
                _animator.SetBool(_animIdWalk, false);
                _isReachedPlayer = true;
                _enemyAttackScript.StartAttack();
            }
            _transform.LookAt(LookAtTarget());
        }
        else
        {
            _animator.SetBool(_animIdWalk, true);
            _isReachedPlayer = false;
        }
    }

    public bool IsReachedPlayer()
    {
        return _isReachedPlayer;
    }

    private void Stop()
    {
        _isStopped = true;
        _animator.SetBool(_animIdWalk, false);
        _enemyAttackScript.StopAttack();
    }

    public void SetSpeedModifier(float modifier, float duration)
    {
        _agent.speed = _enemyStatsSO.Speed * (_speedModifier + modifier);
        StartCoroutine(UnsetMovementModifier(duration));
    }

    private IEnumerator UnsetMovementModifier(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _agent.speed = _enemyStatsSO.Speed * _speedModifier;
    }

}
