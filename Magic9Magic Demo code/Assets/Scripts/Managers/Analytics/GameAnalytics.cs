using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GameAnalytics : MonoBehaviour
{
    [SerializeField] private AnalyticsStorage _analyticsStorageSO;
    private bool _isInitialized;

    public void Initialize()
    {
        if (_isInitialized) return;
        FindAndTestComponents();
        _isInitialized = true;
    }

    private void FindAndTestComponents()
    {
        Tools.CheckSingleInstance<GameAnalytics>(gameObject);
        if (_analyticsStorageSO == null)
            Tools.LogError("AnalyticsStorage _analyticsStorageSO = NULL");
    }

    public void TakeNextFirstStep(int stepNumber)
    {
        if (stepNumber == _analyticsStorageSO.FirstPathStep)
        {
            _analyticsStorageSO.FirstPathStep++;
            Analytics.CustomEvent("FirstPath", new Dictionary<string, object> { { "step", _analyticsStorageSO.FirstPathStep } });
        }
    }

    public bool IsCurrentFirstStep(int stepNumber)
    {
        return stepNumber == _analyticsStorageSO.FirstPathStep;
    }

    public void ChangeColorBlindType(string colorType)
    {
        if (_analyticsStorageSO.FirstPathStep == 2)
            Analytics.CustomEvent("ColorBlindType", new Dictionary<string, object> { { "type", colorType } });
    }

    public void PassedThroughPortal(string worldSceneName, string worldSOName)
    {
        if (worldSceneName.Equals("CampScene") == false)
        {
            TakeNextFirstStep(3);
            Analytics.CustomEvent("WorldName", new Dictionary<string, object> { { "type", worldSceneName }, { "name", worldSOName } });
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
        _analyticsStorageSO.NumberOfBattlesTotal++;
        Analytics.CustomEvent("numberOfBattlesTotal", new Dictionary<string, object> { { "number", _analyticsStorageSO.NumberOfBattlesTotal } });
    }

    private void NumberOfBattlesWorlds(string worldSceneName)
    {
        if (worldSceneName == "World1Scene")
        {
            _analyticsStorageSO.NumberOfBattlesWorld1++;
            Analytics.CustomEvent("numberOfBattlesWorlds", new Dictionary<string, object> { { "World1", _analyticsStorageSO.NumberOfBattlesWorld1 } });
        }
        else
        {
            _analyticsStorageSO.NumberOfBattlesWorld2++;
            Analytics.CustomEvent("numberOfBattlesWorlds", new Dictionary<string, object> { { "World2", _analyticsStorageSO.NumberOfBattlesWorld2 } });
        }        
    }

    public void SendWorldFinishInfo(string worldName, string worldSOName, string missionStatus, int gameDifficulty)
    {
        Analytics.CustomEvent("WorldFinishInfo", new Dictionary<string, object>
        {
            { "worldName", worldName },
            { "worldSOName", worldSOName },
            { "missionStatus", missionStatus },
            { "gameDifficulty", gameDifficulty }
        });
        Analytics.CustomEvent("TotalMissionStatus", new Dictionary<string, object> { { "missionStatus", missionStatus } });
    }

    public void SendMissionTypeStatus(string missionType, int gameDifficulty, string missionStatus)
    {
        Analytics.CustomEvent("MissionTypeStatus", new Dictionary<string, object>
        {
            { "missionType", missionType },
            { "gameDifficulty", gameDifficulty },
            { "missionStatus", missionStatus }
        });
    }

    public void SendMissionParamsStatus(string missionParams, int gameDifficulty, string missionStatus)
    {
        Analytics.CustomEvent("MissionParamsStatus", new Dictionary<string, object>
        {
            { "missionParams", missionParams },
            { "gameDifficulty", gameDifficulty },
            { "missionStatus", missionStatus }
        });
    }

}
