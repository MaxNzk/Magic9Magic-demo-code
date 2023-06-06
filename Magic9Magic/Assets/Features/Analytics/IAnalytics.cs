public interface IAnalytics
{
    void Initialize(AnalyticsStorage analyticsStorageSO);

    public void StartNewGameSession();
    public void TakeNextFirstStep(int stepNumber);
    public void ChangeColorBlindType(string colorType);
    public void PassedThroughPortal(string worldSceneName, string worldSOName);
    public void SendWorldFinishInfo(string worldName, string worldSOName, string missionStatus, int gameDifficulty);
    public void SendMissionTypeStatus(string missionType, int gameDifficulty, string missionStatus);
    public void SendMissionParamsStatus(string missionParams, int gameDifficulty, string missionStatus);
    public void SendNewPlayerLevel(int level);
    public void SendNumberOfDeaths();
}