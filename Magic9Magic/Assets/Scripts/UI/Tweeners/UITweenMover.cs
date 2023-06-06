using System.Collections;
using UnityEngine;

public class UITweenMover : MonoBehaviour
{
    [SerializeField] private bool _isOnAwake = true;
    
    [SerializeField] private float _startDelay;
    [SerializeField] private bool _isHiddenBeforeMoving;
    private Transform _transform;
    private Vector3 _mainTransformScale;

    [Space(10)]
    [Header("Move to Start position ------------------------------------------")]
    [SerializeField] private bool _isMoveToStartPosition;
    [SerializeField] private Vector3 _from;
    private Vector3 _mainPosition;
    [SerializeField] private float _toStartDuration;
    [SerializeField] private bool _isUseStartPathCurve;
    [SerializeField] private AnimationCurve _startPathCurve;

    [Space(10)]
    [Header("Move to Finish position ------------------------------------------")]
    [SerializeField] private bool _isMoveFromStartToFinish;
    [SerializeField] private Vector3 _to;
    [SerializeField] private float _toFinishDuration;
    [SerializeField] private bool _isUseFinishPathCurve;
    [SerializeField] private AnimationCurve _finishPathCurve;

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (_startDelay < 0)
        {
            _startDelay = 0;
        }
        if (_toStartDuration < 0)
        {
            _toStartDuration = 0;
        }
        if (_toFinishDuration < 0)
        {
            _toFinishDuration = 0;
        }
        if (_isMoveToStartPosition && _isMoveFromStartToFinish)
        {
            _isMoveToStartPosition = false;
            _isMoveFromStartToFinish = false;
        }
    }
    #endif

    private void Start()
    {
        _transform = GetComponent<RectTransform>();
        _mainPosition = _transform.position;
        if (_isHiddenBeforeMoving)
        {
            Hide();
        }

        if (_isOnAwake)
        {
            if (_isMoveToStartPosition)
            {
                MoveToStartPosition();
            }
            if (_isMoveFromStartToFinish)
            {
                MoveToFinishPosition();
            }
        }
    }

    private IEnumerator MoveCoroutine(Vector3 startPos, Vector3 finishPos, float duration, bool isUsePathCurve, AnimationCurve pathCurve)
    {
        if (_startDelay > 0)
        {
            yield return new WaitForSeconds(_startDelay);
        }
        if (_isHiddenBeforeMoving)
        {
            Show();
        }

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            if (isUsePathCurve)
            {
                _transform.position = Vector3.Lerp(startPos, finishPos, pathCurve.Evaluate(elapsedTime / duration));
            }
            else
            {
                _transform.position = Vector3.Lerp(startPos, finishPos, elapsedTime / duration);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void Hide()
    {
        _mainTransformScale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }

    private void Show()
    {
        gameObject.transform.localScale = _mainTransformScale;
    }

    public void MoveToStartPosition()
    {
        _transform.position = _from;
        StartCoroutine(MoveCoroutine(_from, _mainPosition, _toStartDuration, _isUseStartPathCurve, _startPathCurve));
    }

    public void MoveToFinishPosition()
    {
        StartCoroutine(MoveCoroutine(_mainPosition, _to, _toFinishDuration, _isUseFinishPathCurve, _finishPathCurve));
    }

}
