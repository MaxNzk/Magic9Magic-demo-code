using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour, IPortalable
{
    [SerializeField] private Animator _animator;
    private int _animIdSpeed = Animator.StringToHash("speed");
    private int _animIdMotionSpeed = Animator.StringToHash("motionSpeed");
    [SerializeField] private bool _isAlwaysRunning;
    [SerializeField] private float _speedRotation = 800f;
    [SerializeField] private float _defaultMotionSpeed = 0.8f;
    private float _currentMotionSpeed;
    [SerializeField] private float _walkSpeed = 5.5f;
    [SerializeField] private float _runSpeed = 12f;
    private float _walkSpeedAnimatorParam = 1.0f;
    private float _runSpeedAnimatorParam = 2.0f;
    
    [SerializeField] private ClickMarker _clickMarker;
    [Range(0.2f, 2.0f)]
    [SerializeField] private float _clickMarkerRepeatTime = 0.5f;
    private float _clickMarkerElapsedTime;
    private Vector3 _clickMarkerPosition;

    private Camera _camera;
    private NavMeshAgent _agent;
    private PlayerTakeDamage _playerTakeDamageScript;
    private Vector3 _target;

    private InputManager _inputManager;

    public void Initialize(InputManager inputManager)
    {
        _inputManager = inputManager;
        FindAndTestComponents();
        _agent.updateRotation = false;
        _agent.speed = _isAlwaysRunning ? _runSpeed : _walkSpeed;
        _currentMotionSpeed = _defaultMotionSpeed;
    }

    private void FindAndTestComponents()
    {
        _camera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
        _playerTakeDamageScript = GetComponent<PlayerTakeDamage>();
        if (_agent == null)
            Tools.LogError("NavMeshAgent _agent = NULL");
        if (_playerTakeDamageScript == null)
            Tools.LogError("PlayerTakeDamage _playerTakeDamageScript = NULL");
    }

    private void Update()
    {
        if (_playerTakeDamageScript.IsDead())
        {
            StopAgent();
        }
        else
        {
            SetDestination();
            CheckFollowPointer2(Time.deltaTime);
            Move();            
        }
    }

    private void CheckFollowPointer2(float deltaTime)
    {
        if (_inputManager.CanMove == false)
        {
            _clickMarkerElapsedTime = 0;
            return;
        }
        
        _clickMarkerElapsedTime += deltaTime;
        if (_clickMarkerElapsedTime >= _clickMarkerRepeatTime)
        {
            _clickMarker.SetPosition(_clickMarkerPosition);
            _clickMarkerElapsedTime = 0;
        }
    }

    private void SetDestination()
    {
        if (_inputManager.CanMove)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Physics.Raycast(_camera.ScreenPointToRay(_inputManager.MousePosition), out RaycastHit hit, 100))
            {
                _target = hit.point;
                _agent.isStopped = false;
                _agent.SetDestination(hit.point);

                if (_clickMarkerElapsedTime == 0)
                    _clickMarker.SetPosition(new Vector3(hit.point.x, hit.point.y, hit.point.z));
                _clickMarkerPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }
        }
    }

    private void Move()
    {
        if (_agent.isStopped == false)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                _agent.updateRotation = false;
                _animator.SetFloat(_animIdSpeed, 0);
                _clickMarkerElapsedTime = 0;
            }
            else
            {
                LookAtDestination(_target);
                if (_isAlwaysRunning)
                {
                    _animator.SetFloat(_animIdSpeed, _runSpeedAnimatorParam);
                    _animator.SetFloat(_animIdMotionSpeed, _currentMotionSpeed);
                }
                else
                {
                    _animator.SetFloat(_animIdSpeed, _walkSpeedAnimatorParam);
                }
            }
        }
    }

    private void LookAtDestination(Vector3 target)
    {
        if (target == Vector3.zero) return;
        Vector3 diraction = (target - transform.position).normalized;
        diraction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(diraction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * _speedRotation);
    }

    public void StopAndLookAtTarget(Vector3 target)
    {
        StopAgent();
        transform.LookAt(target);
    }

    public void Disappear()
    {
        StartCoroutine(DisappearCoroutine());
    }

    private IEnumerator DisappearCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        StopAgent();
        yield return new WaitForSeconds(0.4f);
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
    }

    private void StopAgent()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        _animator.SetFloat(_animIdMotionSpeed, _currentMotionSpeed);
        _animator.SetFloat(_animIdSpeed, 0);
    }

}
