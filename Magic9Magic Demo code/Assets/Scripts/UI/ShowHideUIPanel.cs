using UnityEngine;

public class ShowHideUIPanel : MonoBehaviour
{
    [SerializeField] private bool _isShowOnAwake;

    private void Awake()
    {
        Hide();
        if (_isShowOnAwake)
            Show();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
