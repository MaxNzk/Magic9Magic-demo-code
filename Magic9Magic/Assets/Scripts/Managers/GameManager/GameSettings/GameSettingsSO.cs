using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings")]
public class GameSettingsSO : ScriptableObject
{
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

    [Header("Version: ------------------------------------------")]
    public float CurrentVersion;
    public bool IsCheckForUpdates;

    [Header("Tutorials: ------------------------------------------")]
    public bool IsTutorialWorld1Input1;
    public bool IsTutorialWorld2Input1;

    [Header("Loot: -----------------------------------------------")]
    public bool IsLootCampInit;
}
