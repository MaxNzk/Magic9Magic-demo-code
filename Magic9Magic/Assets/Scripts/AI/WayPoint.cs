using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] private Color _color = Color.red;
    [SerializeField] private float _radius = 0.5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, _radius);
    }

}
