using UnityEngine;
using TMPro;

public class TextBackground : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransformBackground;
    [SerializeField] private TextMeshProUGUI _text;
    private RectTransform _rectTransform;
    private RectTransform _rectTransformText;

    [Space(10)]
    [SerializeField] private int _offsetX;
    [SerializeField] private int _offsetY;

    [Space(10)]
    [SerializeField] private bool _isPosYMoved;
    
    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransformText = _text.GetComponent<RectTransform>();  
    }

    public void UpdateSize()
    {
        int lineNum = Mathf.CeilToInt(_text.preferredHeight / _rectTransformText.sizeDelta.y);

        float x;
        if (lineNum > 1 || _text.preferredWidth > Screen.width)
        {
            x = _rectTransform.sizeDelta.x;
        }
        else
        {
            x = _text.preferredWidth;
        }
        x += _offsetX;
        _rectTransformBackground.sizeDelta = new Vector2(x, _text.preferredHeight + _offsetY);

        if (_isPosYMoved)
        {
            float h2 = _text.preferredHeight / lineNum / 2;
            lineNum--;
            if (lineNum > 0)
            {
                float newPosY = _rectTransformText.anchoredPosition.y + h2 * lineNum;
                _rectTransformText.anchoredPosition = new Vector3(0, newPosY, 0);
            }
        }
        
        _rectTransformBackground.anchoredPosition = new Vector3(0, _rectTransformText.anchoredPosition.y, 0);
    }

}
