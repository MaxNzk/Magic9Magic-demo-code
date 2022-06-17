using UnityEngine;

[CreateAssetMenu(fileName = "WorldPortalColorSO", menuName = "Scriptable Objects/WorldPortalColorSO")]
public class WorldPortalColorSO : ScriptableObject
{
    [field: SerializeField] public string SceneName { get; private set; }
    [field: SerializeField] public Color PortalColor { get; private set; }
    
}
