using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [field: SerializeField] public EnemyStatsSO StatsSO { get; private set; }

    private void Start()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckNull<EnemyStatsSO>(StatsSO, nameof(StatsSO), gameObject);
    }
}
