using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pool", menuName = "Scriptable Objects/Object Pooling/New Pool")]
public class PoolObjectSO : ScriptableObject
{
	public GameObject ObjectPrefab;
	public int ObjectAmount;
	public int MaxObjectAmount;
	[HideInInspector] public int CurrentObjectAmount;
	public Queue<GameObject> PooledObjectQueue;

	#if UNITY_EDITOR
	private void OnValidate()
	{
		if (ObjectAmount < 1)
		{
			ObjectAmount = 1;
		}
		if (ObjectAmount > MaxObjectAmount)
		{
			MaxObjectAmount = ObjectAmount;
		}
		CurrentObjectAmount = ObjectAmount;
	}
	#endif
}
