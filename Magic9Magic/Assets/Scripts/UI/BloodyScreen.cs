using UnityEngine;
using UnityEngine.UI;

public class BloodyScreen : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private AnimationCurve _alphaChannelCurve;

    private float _currentTime;
    private bool _isStarted;

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckNull<Image>(_image, nameof(_image), gameObject);
    }

    private void Update()
    {
        if (_isStarted)
        {
            float t = Mathf.Clamp(_alphaChannelCurve.Evaluate(_currentTime), 0, 1f);
            _image.color = new Color(1f, 0, 0, t);
            _currentTime += Time.deltaTime;
            if (_currentTime >= 1f)
            {
                Hide();
            }
        }
    }

    public void Show()
    {
        _image.gameObject.SetActive(true);
        _image.color = new Color(1f, 0, 0, 0);
        _currentTime = 0;
        _isStarted = true;
    }

    private void Hide()
    {
        _isStarted = false;
        _image.gameObject.SetActive(false);
    }

}
