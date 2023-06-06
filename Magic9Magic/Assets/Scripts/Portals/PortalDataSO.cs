using UnityEngine;

[CreateAssetMenu(fileName = "PortalDataSO", menuName = "Scriptable Objects/PortalDataSO")]
public class PortalDataSO : ScriptableObject
{
    [field: SerializeField] public string SceneName { get; private set; }
    [field: SerializeField] public Material PortalMaterial { get; private set; }

    [field: SerializeField, Space(10)] public int ClosePrice { get; private set; }
    [field: SerializeField] public int MiddlePrice { get; private set; }
    [field: SerializeField] public int FarPrice { get; private set; }
    
}
