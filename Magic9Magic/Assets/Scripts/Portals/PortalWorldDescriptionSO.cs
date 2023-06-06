using UnityEngine;

[CreateAssetMenu(fileName = "PortalWorldDescriptionSO", menuName = "Scriptable Objects/PortalWorldDescriptionSO")]
public class PortalWorldDescriptionSO : ScriptableObject
{
    [field: SerializeField] public string WorldName { get; private set; }
    [field: SerializeField, TextArea(10, 20)] public string WorldDescription { get; private set; }

}
