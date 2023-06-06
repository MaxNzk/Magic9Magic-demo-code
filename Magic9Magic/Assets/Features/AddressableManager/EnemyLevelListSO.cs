using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyLevel
{
    [field: SerializeField] public ReferenceNameSO NameSO { get; private set; }
    [field: SerializeField] public int ObjectAmount { get; private set; }
	[field: SerializeField] public int MaxObjectAmount { get; private set; }
    [field: SerializeField] public float Probability { get; private set; }

    [HideInInspector] public int CurrentObjectAmount;
	public Queue<GameObject> PooledObjectQueue;
}

[CreateAssetMenu(fileName = "EnemyLevelListSO", menuName = "Scriptable Objects/EnemyLevelListSO")]
public class EnemyLevelListSO : ScriptableObject
{
    [field: SerializeField] public EnemyLevel[] EnemyLevelList { get; private set; }
}
