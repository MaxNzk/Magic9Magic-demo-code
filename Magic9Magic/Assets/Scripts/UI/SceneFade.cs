using UnityEngine;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    [SerializeField] private Color32 _color;
    [SerializeField] private AnimationCurve _fadeOutCurve;
    [SerializeField] private AnimationCurve _fadeInCurve;
    [SerializeField] private AnimationCurve _fastFadeInCurve;

    private float _currentTime;
    private float _totalTime;
    private bool _startScene;
    private bool _fastFadeInScene;
    private CanvasGroup _canvasGroup;
    private Image _panel;
    private bool isFinished = true;

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        _canvasGroup = Tools.GetComponentInChildrenWithAssertion<CanvasGroup>(gameObject);
        _panel = Tools.GetComponentInChildrenWithAssertion<Image>(gameObject);
    }

    private void Start()
    {
        _panel.color = _color;
    }

    private void Update()
    {
        if (isFinished) 
            return;

        if (_fastFadeInScene)
        {
            _canvasGroup.alpha = Mathf.Clamp(_fastFadeInCurve.Evaluate(_currentTime), 0, float.MaxValue);
            _currentTime += Time.deltaTime;
            if (_currentTime >= _totalTime)
            {
                _currentTime = _totalTime;
                _canvasGroup.alpha = _fastFadeInCurve.Evaluate(_currentTime);
                _fastFadeInScene = false;
                isFinished = true;
            }
            return;
        }

        if (_startScene)
        {
            _canvasGroup.alpha = Mathf.Clamp(_fadeOutCurve.Evaluate(_currentTime), 0, float.MaxValue);
            _currentTime += Time.deltaTime;
            if (_currentTime >= _totalTime)
            {
                _currentTime = _totalTime;
                _canvasGroup.alpha = _fadeOutCurve.Evaluate(_currentTime);
                isFinished = true;
                gameObject.SetActive(false);
            }
        }
        else
        {
            _canvasGroup.alpha = Mathf.Clamp(_fadeInCurve.Evaluate(_currentTime), 0, float.MaxValue);
            _currentTime += Time.deltaTime;
            if (_currentTime >= _totalTime)
            {
                _currentTime = _totalTime;
                _canvasGroup.alpha = _fadeInCurve.Evaluate(_currentTime);
                isFinished = true;
            }
        }
    }

    public void FadeOut()
    {
        gameObject.SetActive(true);
        _totalTime = _fadeOutCurve.keys[_fadeOutCurve.keys.Length - 1].time;
        _canvasGroup.alpha = 1;
        _currentTime = 0;
        _startScene = true;
        isFinished = false;
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);
        _totalTime = _fadeInCurve.keys[_fadeInCurve.keys.Length - 1].time;
        _canvasGroup.alpha = 0;
        _currentTime = 0;
        _startScene = false;
        isFinished = false;
    }

    public void FastFadeIn()
    {
        gameObject.SetActive(true);
        _totalTime = _fastFadeInCurve.keys[_fastFadeInCurve.keys.Length - 1].time;
        _canvasGroup.alpha = 0;
        _currentTime = 0;
        _fastFadeInScene = true;
        isFinished = false;
    }

}
