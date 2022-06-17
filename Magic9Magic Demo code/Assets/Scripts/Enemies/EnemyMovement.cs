using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] EnemySettings _enemySettingsSO;
    private Transform _transform;
    private NavMeshAgent _agent;
    private Animator _animator;
    private int _animIdWalk = Animator.StringToHash("walk");
    private Transform _player;
    private PlayerTakeDamage _playerTakeDamageScript;
    private bool _isReachedPlayer;
    private bool _isStopped;
    private EnemyAttack _enemyAttackScript;
    private EnemyTakeDamage _enemyTakeDamageScript;
    private float _speedModifier = 1.0f;
    
    public void Start()
    {
        FindAndTestComponents();
        SetupAgent();        
    }

    private void OnEnable()
    {
        _isStopped = false;
        _isReachedPlayer = false;
    }

    private void FindAndTestComponents()
    {
        _transform = GetComponent<Transform>();
        _enemyAttackScript = GetComponent<EnemyAttack>();
        _enemyTakeDamageScript = GetComponent<EnemyTakeDamage>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<Transform>();
        _playerTakeDamageScript = FindObjectOfType<PlayerTakeDamage>();

        if (_enemyAttackScript == null)
            Tools.LogError("EnemyAttack _enemyAttackScript = NULL");
        if (_enemyTakeDamageScript == null)
            Tools.LogError("EnemyTakeDamage _enemyTakeDamageScript = NULL");
        if (_agent == null)
            Tools.LogError("NavMeshAgent _agent = NULL");
        if (_animator == null)
            Tools.LogError("Animator _animator = NULL");
        if (_player == null)
            Tools.LogError("Transform _player = NULL");
        if (_playerTakeDamageScript == null)
            Tools.LogError("PlayerTakeDamage _playerTakeDamageScript = NULL");
    }

    private void SetupAgent()
    {
        _agent.speed = _enemySettingsSO.Speed;
        _agent.stoppingDistance = _enemySettingsSO.StoppingDistance;
        _isReachedPlayer = true; // anti-first call: if (_isReachedPlayer == false)
    }

    private void Update()
    {
        if (CanMove() == false) return;
        SetDestination();
        Move();
    }

    private bool CanMove()
    {
        if (_isStopped || _enemyTakeDamageScript.IsDead || _playerTakeDamageScript.IsDead())
        {
            _agent.isStopped = true;
            return false;
        }
        return true;
    }

    private void SetDestination()
    {
        _agent.SetDestination(_player.position);
    }

    private void Move()
    {
        if (_agent.pathPending) return;

        // Debug.Log("remainingDistance = " + _agent.remainingDistance.ToString());
        // Debug.Log("stoppingDistance = " + _agent.stoppingDistance.ToString());
        // Debug.Log("--------------------------------------------");

        if (_agent.remainingDistance < _agent.stoppingDistance)
        {
            _animator.SetBool(_animIdWalk, false);
            if (_isReachedPlayer == false)
            {
                _isReachedPlayer = true;
                _enemyAttackScript.StartAttack();
            }
            LookAtPlayer();
        }
        else
        {
            _animator.SetBool(_animIdWalk, true);
            _isReachedPlayer = false;
        }
    }

    private void LookAtPlayer()
    {
        _transform.LookAt(_player);
    }

    public bool IsReachedPlayer()
    {
        return _isReachedPlayer;
    }

    public void Stop()
    {
        _isStopped = true;
        _animator.SetBool(_animIdWalk, false);
    }

    public void SetSpeedModifier(float modifier, float duration)
    {
        _agent.speed = _enemySettingsSO.Speed * (_speedModifier + modifier);
        StartCoroutine(UnsetMovementModifier(duration));
    }

    private IEnumerator UnsetMovementModifier(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _agent.speed = _enemySettingsSO.Speed * _speedModifier;
    }

}
