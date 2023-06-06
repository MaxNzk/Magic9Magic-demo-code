using UnityEngine;

[CreateAssetMenu(fileName = "ReferenceNameSO", menuName = "Scriptable Objects/ReferenceNameSO")]
public class ReferenceNameSO : ScriptableObject
{
    [field: SerializeField] public string ReferenceName { get; private set; }
}
