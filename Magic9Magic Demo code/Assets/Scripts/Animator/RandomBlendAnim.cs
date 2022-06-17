using UnityEngine;

public class RandomBlendAnim : MonoBehaviour
{
    [SerializeField] private float _minShiftTime = 10.0f;
    [SerializeField] private float _maxShiftTime = 15.0f;
    private Animator _animator;
    private int _numberOfAnimations = 2;
    private int _animIdleIndexID = Animator.StringToHash("idleIndex");
    private int _animNewIdleID = Animator.StringToHash("newIdle");
    private int _animIndex;
    private int _prvAnimIndex = 1;
    private float _waitTime;
    private float t;

    private void Awake()
    {
        FindAndTestComponents();
        _waitTime = Random.Range(_minShiftTime, _maxShiftTime);
    }

    private void FindAndTestComponents()
    {
        _animator = GetComponent<Animator>();
        if (_animator == null)
            Tools.LogError("Animator _animator = NULL");
    }

    private void Update()
    {
        t += Time.deltaTime;
        if (t > _waitTime)
        {
            t = 0;
            _waitTime = Random.Range(_minShiftTime, _maxShiftTime);
            _animIndex = Random.Range(0, _numberOfAnimations);
            while (_animIndex == _prvAnimIndex)
                _animIndex = Random.Range(0, _numberOfAnimations);
            _prvAnimIndex = _animIndex;
            _animator.SetInteger(_animIdleIndexID, _animIndex);
            _animator.SetTrigger(_animNewIdleID);
        }
    }
}
