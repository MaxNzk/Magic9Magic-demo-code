using UnityEngine;

[CreateAssetMenu(fileName = "AnalyticsStorage", menuName = "Scriptable Objects/AnalyticsStorage")]
public class AnalyticsStorage : ScriptableObject
{
    public int PlayerLevel;
    public int GameSession;
    public int NumberOfDeaths;
    public int FirstPathStep;

    [Header("Battles: ------------------------------------------")]
    public int NumberOfBattlesTotal;
    public int NumberOfBattlesWorld1;
    public int NumberOfBattlesWorld1v2;
    public int NumberOfBattlesWorld2;
    
    [Header("Mission Status: ------------------------------------------")]
    public int MissionWorld1Completed;
    public int MissionWorld1Failed;
    public int MissionWorld1v2Completed;
    public int MissionWorld1v2Failed;
    public int MissionWorld2Completed;
    public int MissionWorld2Failed;

    [Header("Portals: ------------------------------------------")]
    public int[] ThroughPortalsAmount;
    public int PassedThroughPortalToWorld;
}
