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
        Tools.CheckNull<Slider>(_slider, nameof(_slider), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_textValue, nameof(_textValue), gameObject);
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
