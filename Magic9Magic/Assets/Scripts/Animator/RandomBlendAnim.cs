using UnityEngine;

public class RandomBlendAnim : MonoBehaviour
{
    [SerializeField] private int _numberOfAnimations = 2;
    [SerializeField] private float _minAnimChangeTime = 10.0f;
    [SerializeField] private float _maxAnimChangeTime = 15.0f;

    private readonly int _triggerNewAnimId = Animator.StringToHash("triggerNewAnim");
    private readonly int _indexNewAnimId = Animator.StringToHash("indexNewAnim");

    private Animator _animator;
    private int _animIndex;
    private int _prvAnimIndex = 1;
    private float _animChangeTime;
    private float _elapsedTime;

    private void Awake()
    {
        FindAndTestComponents();
        _animChangeTime = Random.Range(_minAnimChangeTime, _maxAnimChangeTime);
    }

    private void FindAndTestComponents()
    {
        _animator = Tools.GetComponentWithAssertion<Animator>(gameObject);
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _animChangeTime)
        {
            _elapsedTime = 0;
            _animChangeTime = Random.Range(_minAnimChangeTime, _maxAnimChangeTime);
            _animIndex = Random.Range(0, _numberOfAnimations);

            while (_animIndex == _prvAnimIndex)
            {
                _animIndex = Random.Range(0, _numberOfAnimations);
            }
            
            _prvAnimIndex = _animIndex;
            _animator.SetInteger(_indexNewAnimId, _animIndex);
            _animator.SetTrigger(_triggerNewAnimId);
        }
    }
}
