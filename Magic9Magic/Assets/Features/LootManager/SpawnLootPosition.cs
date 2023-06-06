using System.Collections.Generic;
using UnityEngine;

public class SpawnLootPosition : MonoBehaviour
{
    [SerializeField] private bool _isGizmos;
    [SerializeField] private Vector3 _gizmosSize;
    [SerializeField] private Color _gizmosColor;
    
    [field: SerializeField, Space(10)] public List<LootItemListSO> ItemListsSO { get; private set; }

    private void OnDrawGizmos()
    {
        if (_isGizmos)
        {
            Gizmos.color = _gizmosColor;
            Gizmos.DrawCube(transform.position, _gizmosSize);
        }
    }

}
