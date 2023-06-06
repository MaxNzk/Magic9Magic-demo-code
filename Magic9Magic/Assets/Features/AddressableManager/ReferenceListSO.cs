using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class ObjectReference
{
    [field: SerializeField] public ReferenceNameSO ObjectName { get; private set; }
    [field: SerializeField] public AssetReference AssetRef { get; private set; }
}

[CreateAssetMenu(fileName = "ReferenceListSO", menuName = "Scriptable Objects/ReferenceListSO")]
public class ReferenceListSO : ScriptableObject
{
    [field: SerializeField] public ObjectReference[] ObjectList { get; private set; }
}
