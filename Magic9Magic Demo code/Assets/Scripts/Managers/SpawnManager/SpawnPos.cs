using UnityEngine;

public class SpawnPos : MonoBehaviour
{
    [field: SerializeField] public bool IsActive { get; private set; }
    [SerializeField] private float _scanDistance = 30.0f;
    private bool _isGizmos = true;
    private bool _isGizmosDistance = true;
    private Color _activeGizmosColor = new Color(0, 1, 0, 0.5f);
    private Color _inactiveGizmosColor = new Color(1, 0, 0, 0.5f);
    private Transform _playerTransform;
    private Transform _transform;

    public void Initialize(Transform playerTransform, bool isGizmos, bool isGizmosDistance, Color activeGizmosColor, Color inactiveGizmosColor)
    {
        _playerTransform = playerTransform;
        _isGizmos = isGizmos;
        _isGizmosDistance = isGizmosDistance;
        _activeGizmosColor = activeGizmosColor;
        _inactiveGizmosColor = inactiveGizmosColor;
        IsActive = true;
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        _transform = GetComponent<Transform>();
        if (_playerTransform == null)
            Tools.LogError("Transform _playerTransform = NULL");
    }

    public bool Scan()
    {
        // Debug.Log("Scan()");
        float d = Vector3.Distance(_transform.position, _playerTransform.position);
        // Debug.Log("distance = " + d.ToString());
        if (d < _scanDistance)
            IsActive = false;
        else
            IsActive = true;
        // Debug.Log(IsActive);
        return IsActive;
    }

    private void OnDrawGizmos()
    {
        if (_isGizmos)
        {
            if (IsActive)
                Gizmos.color = _activeGizmosColor;
            else
                Gizmos.color = _inactiveGizmosColor;
            Gizmos.DrawSphere(transform.position, 2.5f);
            if (_isGizmosDistance)
                Gizmos.DrawWireSphere(transform.position, _scanDistance);
        }
    }

}
