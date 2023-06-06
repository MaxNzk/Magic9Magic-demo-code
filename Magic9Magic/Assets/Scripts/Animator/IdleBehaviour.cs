using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    [SerializeField] private float _timeUntilBored;
    [SerializeField] private int _numberOfBoredAnimations;
    [SerializeField] private bool _withoutRepetition = true;
    
    private readonly int _animIdIdleType = Animator.StringToHash("idleType");
    private bool _isBored;
    private float _idleTime;
    private int _boredAnimation;
    private int _boredAnimationPrv;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       InitialIdle(animator);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (_isBored == false)
        {
            _idleTime += Time.deltaTime;

            if (_idleTime > _timeUntilBored && stateInfo.normalizedTime % 1 < 0.02f) 
            {
                _isBored = true;

                _boredAnimation = Random.Range(1, _numberOfBoredAnimations + 1);
                _boredAnimation = _boredAnimation * 2 - 1;
                if (_withoutRepetition)
                {
                    while (_boredAnimationPrv == _boredAnimation)
                    {
                        _boredAnimation = Random.Range(1, _numberOfBoredAnimations + 1);
                        _boredAnimation = _boredAnimation * 2 - 1;
                    }
                    _boredAnimationPrv = _boredAnimation;
                }

                animator.SetFloat(_animIdIdleType, _boredAnimation - 1);
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98)
        {
            ResetIdle();
        }

        animator.SetFloat(_animIdIdleType, _boredAnimation, 0.2f, Time.deltaTime);
    }

    private void ResetIdle()
    {
        if (_isBored)
        {
            _boredAnimation--;
        }

        _isBored = false;
        _idleTime = 0;
    }

    private void InitialIdle(Animator animator)
    {
        _isBored = false;
        _idleTime = 0;
        _boredAnimation = 0;
    }

}
