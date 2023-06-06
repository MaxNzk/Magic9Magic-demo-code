using UnityEngine;

public class FakeAnalytics : MonoBehaviour, IAnalytics
{
    private AnalyticsStorage _analyticsStorageSO;
    private bool _isInitialized;

    public void Initialize(AnalyticsStorage analyticsStorageSO)
    {
        if (_isInitialized)
            return;

        _analyticsStorageSO = analyticsStorageSO;
        FindAndTestComponents();
        _isInitialized = true;
    }

    private void FindAndTestComponents()
    {
        Tools.CheckSingleInstance<FakeAnalytics>(gameObject);
    }

    public void StartNewGameSession()
    {
        _analyticsStorageSO.GameSession++;
    }

    public void TakeNextFirstStep(int stepNumber)
    {
        if (stepNumber == _analyticsStorageSO.FirstPathStep)
        {
            Debug.Log("TakeNextFirstStep = " + stepNumber.ToString());
            _analyticsStorageSO.FirstPathStep++;
        }
    }

    public void ChangeColorBlindType(string colorType)
    {
        Debug.Log("FakeAnalytics: ChangeColorBlindType = " + colorType);
    }

    public void PassedThroughPortal(string worldSceneName, string worldSOName)
    {
        if (worldSceneName.Equals("CampScene") == false)
        {
            TakeNextFirstStep(3);

            _analyticsStorageSO.PassedThroughPortalToWorld++;

            SendNumberOfBattlesTotal();
            NumberOfBattlesWorlds(worldSceneName);
        }
        else
        {
            TakeNextFirstStep(5);
        }
    }

    private void SendNumberOfBattlesTotal()
    {
        Debug.Log("FakeAnalytics: SendNumberOfBattlesTotal()");
        _analyticsStorageSO.NumberOfBattlesTotal++;
    }

    private void NumberOfBattlesWorlds(string worldSceneName)
    {
        Debug.Log("FakeAnalytics: NumberOfBattlesWorlds() worldSceneName = " + worldSceneName);
        if (worldSceneName == "World1Scene")
        {
            _analyticsStorageSO.NumberOfBattlesWorld1++;
        }
        if (worldSceneName == "World1v2Scene")
        {
            _analyticsStorageSO.NumberOfBattlesWorld1v2++;
        }
        if (worldSceneName == "World2Scene")
        {
            _analyticsStorageSO.NumberOfBattlesWorld2++;
        }        
    }

    public void SendWorldFinishInfo(string worldName, string worldSOName, string missionStatus, int gameDifficulty)
    {
        Debug.Log("FakeAnalytics: SendWorldFinishInfo() worldName = " + worldName + ", worldSOName = " + worldSOName + ", missionStatus = " + missionStatus + ", gameDifficulty = " + gameDifficulty.ToString());
    }

    public void SendMissionTypeStatus(string missionType, int gameDifficulty, string missionStatus)
    {
        Debug.Log("FakeAnalytics: SendMissionTypeStatus() missionType = " + missionType + ", missionStatus = " + missionStatus + ", gameDifficulty = " + gameDifficulty.ToString());
    }

    public void SendMissionParamsStatus(string missionParams, int gameDifficulty, string missionStatus)
    {
        Debug.Log("FakeAnalytics: SendMissionParamsStatus() missionParams = " + missionParams + ", missionStatus = " + missionStatus + ", gameDifficulty = " + gameDifficulty.ToString());
    }

    public void SendNewPlayerLevel(int level)
    {
        Debug.Log("FakeAnalytics: SendNewPlayerLevel() level = " + level.ToString());
    }

    public void SendNumberOfDeaths()
    {
        _analyticsStorageSO.NumberOfDeaths++;
        Debug.Log("FakeAnalytics: SendNumberOfDeaths() NumberOfDeaths = " + _analyticsStorageSO.NumberOfDeaths);
    }

}
