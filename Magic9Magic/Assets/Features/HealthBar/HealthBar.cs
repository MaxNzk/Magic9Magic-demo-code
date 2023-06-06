using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider _slider;
    private float _autoHideAfterSec;

    public void Initialize(int maxSliderValue, bool isShowOnAwake, float autoHideAfterSec)
    {
        FindAndTestComponents();
        SetupSlider(maxSliderValue);
        if (autoHideAfterSec > 0)
        {
            _autoHideAfterSec = autoHideAfterSec;
            isShowOnAwake = false;
        }
        gameObject.SetActive(isShowOnAwake);
    }

    private void FindAndTestComponents()
    {
        _slider = GetComponentInChildren<Slider>();
        if (_slider == null)
            Tools.LogError("Slider _slider = NULL");
    }

    private void SetupSlider(int maxValue)
    {
        _slider.maxValue = maxValue;
        _slider.value = maxValue;
    }

    private IEnumerator Hide(float autoHideAfterSec)
    {
        yield return new WaitForSeconds(autoHideAfterSec);
        gameObject.SetActive(false);
    }

    public void SetValue(int value)
    {
        _slider.value = value;
        bool isShow = value != 0;
        gameObject.SetActive(isShow);
        if (_autoHideAfterSec > 0 && isShow)
        {
            StartCoroutine(Hide(_autoHideAfterSec));
        }
    }

}
