using System.Collections;
using UnityEngine;

public class UITweenScaler : MonoBehaviour
{
    [SerializeField] private bool _isOnAwake = true;
    [SerializeField] private float _scalingDuration;
    [SerializeField] private float _startScaleDelay;
    [SerializeField] private AnimationCurve _scalingPathCurve;
    private Transform _transform;

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (_scalingDuration < 0)
        {
            _scalingDuration = 1.0f;
        }
        if (_startScaleDelay < 0)
        {
            _startScaleDelay = 0;
        }
    }
    #endif

    private void Start()
    {
        _transform = GetComponent<RectTransform>();
        if (_isOnAwake)
        {
            StartCoroutine(ScaleCoroutine(_scalingDuration, _scalingPathCurve));
        }
    }

    public void StartScale()
    {
        StartCoroutine(ScaleCoroutine(_scalingDuration, _scalingPathCurve));
    }
    
    private IEnumerator ScaleCoroutine(float duration, AnimationCurve pathCurve)
    {
        if (_startScaleDelay > 0)
        {
            yield return new WaitForSeconds(_startScaleDelay);
        }

        float elapsedTime = 0f;
        float xyz = 0f;
        Vector3 originScale = _transform.localScale;

        while (elapsedTime < duration)
        {
            xyz = pathCurve.Evaluate(elapsedTime / duration);
            _transform.localScale = new Vector3(xyz, xyz, xyz);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        _transform.localScale = originScale;
    }

}
