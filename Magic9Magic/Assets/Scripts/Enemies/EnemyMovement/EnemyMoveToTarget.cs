using UnityEngine;

public class EnemyMoveToTarget : MonoBehaviour
{
    private Transform _player;

    public void Initialize(Transform player)
    {
        _player = player;
    }

    public Vector3 GetDestination()
    {
        if (_player == null)
        {
            Tools.LogError("EnemyMoveToTarget: GetDestination() return Vector3.zero");
            return Vector3.zero;
        }
        return _player.position;
    }

    public Transform LookAtTarget()
    {
        return _player;
    }

}
