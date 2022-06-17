using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _textValue;
    private int _maxValue;

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        if (_slider == null)
            Tools.LogError("Slider _slider = NULL");
        if (_textValue == null)
            Tools.LogError("TextMeshProUGUI _textValue = NULL");
    }

    public void SetMaxValue(int maxValue)
    {
        _maxValue = maxValue;
        _slider.maxValue = _maxValue;
    }
    
    public void SetValue(int value)
    {
        _slider.value = value;
        _textValue.text = value.ToString() + "/" + _maxValue.ToString();
    }

}
