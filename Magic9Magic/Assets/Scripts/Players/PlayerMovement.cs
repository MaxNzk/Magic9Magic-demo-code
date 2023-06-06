using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour, IPortalable
{
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Animator _animator;
    private readonly int _animIdSpeed = Animator.StringToHash("speed");
    private readonly int _animIdMotionSpeed = Animator.StringToHash("motionSpeed");

    [Space(10)]
    [SerializeField] private bool _isAlwaysRunning;
    [SerializeField] private float _speedRotation = 800f;

    [Space(10)]
    [SerializeField] private float _walkSpeed = 5.5f;
    [SerializeField] private float _walkMotionSpeed = 1f;

    [Space(10)]
    [SerializeField] private float _runSpeed = 12f;
    [SerializeField] private float _runMotionSpeed = 1f;

    private float _currentSpeedAnimatorParam;
    private float _walkSpeedAnimatorParam = 1.0f;
    private float _runSpeedAnimatorParam = 2.0f;
    private float _currentMotionSpeed;
    
    [Space(10)]
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
        SetSpeedParam(isAlwaysRunning: true);
        _agent.updateRotation = false;        
    }

    public void SetSpeedParam(bool isAlwaysRunning)
    {
        _isAlwaysRunning = isAlwaysRunning;
        _agent.speed = _isAlwaysRunning ? _runSpeed : _walkSpeed;
        _currentMotionSpeed = _isAlwaysRunning ? _runMotionSpeed : _walkMotionSpeed;
        _currentSpeedAnimatorParam = _isAlwaysRunning ? _runSpeedAnimatorParam : _walkSpeedAnimatorParam;
    }

    private void FindAndTestComponents()
    {
        _camera = Camera.main;
        _agent = Tools.GetComponentWithAssertion<NavMeshAgent>(gameObject);
        _playerTakeDamageScript = Tools.GetComponentWithAssertion<PlayerTakeDamage>(gameObject);
        Tools.CheckNull<Animator>(_animator, nameof(_animator), gameObject);
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
            CheckFollowPointer(Time.deltaTime);
            Move();            
        }
    }

    private void CheckFollowPointer(float deltaTime)
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
                
            if (Physics.Raycast(_camera.ScreenPointToRay(_inputManager.MousePosition), out RaycastHit hit, 1000, _groundLayerMask))
            {
                _target = hit.point;
                _agent.isStopped = false;
                _agent.SetDestination(hit.point);
                if (_clickMarkerElapsedTime == 0)
                {
                    _clickMarker.SetPosition(hit.point);
                }
                _clickMarkerPosition = hit.point;
            }
        }
    }

    private void Move()
    {
        if (_agent.isStopped)
            return;
        
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            _agent.updateRotation = false;
            _animator.SetFloat(_animIdSpeed, 0);
            _clickMarkerElapsedTime = 0;
        }
        else
        {
            LookAtDestination();
            _animator.SetFloat(_animIdSpeed, _currentSpeedAnimatorParam);
            _animator.SetFloat(_animIdMotionSpeed, _currentMotionSpeed);
        }
    }

    private void LookAtDestination()
    {
        if (_target == Vector3.zero)
            return;
            
        Vector3 diraction = (_target - transform.position).normalized;
        diraction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(diraction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * _speedRotation);
    }

    public void StopAndLookAtTarget(Vector3 target)
    {
        StopAgent();
        transform.LookAt(target);
    }

    public void SetDestinationAtPoint(Vector3 position)
    {
        _target = position;
        _agent.isStopped = false;
        _agent.SetDestination(position);
    }

    public void SetTarget(Transform transformPosition)
    {
        _target = transformPosition.position;
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

    public void StopAgent()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        _animator.SetFloat(_animIdMotionSpeed, _currentMotionSpeed);
        _animator.SetFloat(_animIdSpeed, 0);
    }

}
