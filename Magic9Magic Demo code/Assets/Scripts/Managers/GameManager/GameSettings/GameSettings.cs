using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/GameSettings")]
public class GameSettings : ScriptableObject
{
    [field: SerializeField] public int CurrentVersion { get; private set; }
    public bool IsCheckForUpdates;
    public string Language;
    public int GraphicsLevel;
    public bool IsSound;
    [Range(0, 1.0f)]
    public float MusicVolume;
    [Range(0, 1.0f)]
    public float EffectsVolume;
    [Range(0, 1.0f)]
    public float AmbientVolume;
    public string ColorBlindType;
    public int GameDifficulty;
    public string WorldSOName;
}
