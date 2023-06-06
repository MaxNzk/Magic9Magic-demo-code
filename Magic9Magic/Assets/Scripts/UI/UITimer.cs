using UnityEngine;
using TMPro;

public class UITimer : MonoBehaviour
{
    [SerializeField] private GameObject _timer;
    [SerializeField] private TextMeshProUGUI _timerText;

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckNull<GameObject>(_timer, nameof(_timer), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_timerText, nameof(_timerText), gameObject);
    }
    
    public void Show() => _timer.SetActive(true);
    public void Hide() => _timer.SetActive(false);
    public void SetText(int minutes, int seconds) => _timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

}
