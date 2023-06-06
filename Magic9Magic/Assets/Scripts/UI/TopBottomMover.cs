using UnityEngine;

public class TopBottomMover : MonoBehaviour
{
    [SerializeField] private float _offsetPositionY;
    [SerializeField] private float _speed;
    private RectTransform _rectTransform;

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        _rectTransform = GetComponent<RectTransform>();
        if (_rectTransform == null)
            Tools.LogError("RectTransform _rectTransform = NULL");
    }

    private void Update()
    {
        float currentYPosition = Mathf.PingPong(Time.time * _speed, _offsetPositionY * 2) - _offsetPositionY;

        if (float.IsNaN(currentYPosition))
            return;
        
        _rectTransform.localPosition = new Vector3(0, currentYPosition, 0);
        _rectTransform.SetLeft(50f);
        _rectTransform.SetRight(50f);
    }

}
