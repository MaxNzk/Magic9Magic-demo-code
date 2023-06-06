using UnityEngine;

[CreateAssetMenu(fileName = "GameStateSettings", menuName = "Settings/GameStateSettings")]
public class GameStateSettingsSO : ScriptableObject
{
    public string WorldSOName;
    public int GameDifficulty;
}
