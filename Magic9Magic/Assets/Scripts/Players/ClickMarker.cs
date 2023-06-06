using System.Collections;
using UnityEngine;

public class ClickMarker : MonoBehaviour
{
    [SerializeField] private bool isShown;
    [SerializeField] private GameObject _marker;
    [SerializeField] private float _yPosOffSet = 0.5f;
    [SerializeField] private float _delayBeforDisactivate;
    
    private WaitForSeconds _delay;
    private Transform _transform;

    private void Awake()
    {
        FindAndTestComponents();
        _delay = new WaitForSeconds(_delayBeforDisactivate);
    }

    private void FindAndTestComponents()
    {
        _transform = GetComponent<Transform>();
        Tools.CheckNull<GameObject>(_marker, nameof(_marker), gameObject);
    }
    
    public void SetPosition(Vector3 position)
    {
        if (isShown)
        {
            Vector3 currentPosition = new Vector3(position.x, position.y + _yPosOffSet, position.z);
            _transform.position = currentPosition;
            StartCoroutine(PlayEffect());
        }
    }

    private IEnumerator PlayEffect()
    {
        _marker.SetActive(true);
        yield return _delay;
        _marker.SetActive(false);
    }

}
