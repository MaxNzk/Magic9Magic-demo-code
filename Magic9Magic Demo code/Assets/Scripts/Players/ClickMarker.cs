using System.Collections;
using UnityEngine;

public class ClickMarker : MonoBehaviour
{
    [SerializeField] private bool isShown;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _yPosOffSet = 0.5f;
    private Transform _transform;

    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        _transform = GetComponent<Transform>();
        if (_prefab == null)
            Tools.LogError("GameObject _clickMarker = NULL");
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
        _prefab.SetActive(true);
        yield return new WaitForSeconds(1f);
        _prefab.SetActive(false);
    }

}
