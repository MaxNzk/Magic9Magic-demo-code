using UnityEngine;

public class EnemyPoint : MonoBehaviour
{
    [SerializeField] private Color _color;

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
