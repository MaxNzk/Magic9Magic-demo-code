using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Scriptable Objects/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public string playerName;

    [Space(10)]
    public int MaxHealth = 100;
    public int HealthAlarmLevel = 20;
    public int HealthAlarmRecoveryLevel = 80;
    public int CurrentHealth = 100;

    [Space(10)]
    public int MaxMana = 100;
    public int ManaAlarmLevel = 20;
    public int ManaAlarmRecoveryLevel = 80;
    public int CurrentMana = 100;

    [Space(10)]
    public int MaxLevelPoints = 1000;
    public int XpPoints;
    public int Level = 1;
    public bool IsLevelChanged;

    [Space(10)]
    public int Gold = 100;
    public int SoulStone = 50;

    [Space(10)]
    public int RingSlotAmount = 5;
}
