using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;

public class SingletonAnalytics : MonoBehaviour, IAnalytics
{
    public static SingletonAnalytics Instance { get; private set; }
    private AnalyticsStorage _analyticsStorageSO;

    private void Awake()
    {
        CreateSingleton();
        InitializeAnalyticsService();
    }

    private void CreateSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Tools.CheckSingleInstance<SingletonAnalytics>(gameObject);
            Destroy(gameObject);
        }
    }

    private async void InitializeAnalyticsService()
    {
        // Tools.Log("InitializeAnalyticsService()");
        await UnityServices.InitializeAsync();
        await AnalyticsService.Instance.CheckForRequiredConsents();
    }

    public void Initialize(AnalyticsStorage analyticsStorageSO)
    {
        _analyticsStorageSO = analyticsStorageSO;
    }

    private bool MustBeSent(int num)
    {
        if (num < 10) return true;
        if (num == 20) return true;
        if (num == 50) return true;
        if (num == 100) return true;
        if (num == 200) return true;
        if (num == 500) return true;
        if (num == 1000) return true;

        return false;
    }

    public void StartNewGameSession()
    {
        _analyticsStorageSO.GameSession++;

        if (MustBeSent(_analyticsStorageSO.GameSession) == false)
            return;
        
        AnalyticsService.Instance.CustomData("GameSession", new Dictionary<string, object> { { "gameSessionNum", _analyticsStorageSO.GameSession } });
        AnalyticsService.Instance.Flush();
    }

    public void TakeNextFirstStep(int stepNumber)
    {
        if (stepNumber == _analyticsStorageSO.FirstPathStep)
        {
            Debug.Log("TakeNextFirstStep = " + stepNumber.ToString());
            _analyticsStorageSO.FirstPathStep++;
            AnalyticsService.Instance.CustomData("FirstPath", new Dictionary<string, object> { { "step", _analyticsStorageSO.FirstPathStep } });
            AnalyticsService.Instance.Flush();
        }
    }

    public void ChangeColorBlindType(string colorType)
    {
        if (_analyticsStorageSO.FirstPathStep == 2)
        {
            AnalyticsService.Instance.CustomData("ColorBlindType", new Dictionary<string, object> { { "colorBlindType", colorType } });
            AnalyticsService.Instance.Flush();
        }
    }

    public void PassedThroughPortal(string worldSceneName, string worldSOName)
    {
        if (worldSceneName.Equals("CampScene") == false)
        {
            TakeNextFirstStep(3);

            _analyticsStorageSO.PassedThroughPortalToWorld++;
            if (MustBeSent(_analyticsStorageSO.PassedThroughPortalToWorld))
            {
                AnalyticsService.Instance.CustomData("PassedThroughPortal", new Dictionary<string, object> { { "worldSceneName", worldSceneName }, { "worldSOName", worldSOName } });
            }

            SendNumberOfBattlesTotal();
            NumberOfBattlesWorlds(worldSceneName);

            AnalyticsService.Instance.Flush();
        }
        else
        {
            TakeNextFirstStep(5);
        }
    }

    private void SendNumberOfBattlesTotal()
    {
        _analyticsStorageSO.NumberOfBattlesTotal++;
        if (MustBeSent(_analyticsStorageSO.NumberOfBattlesTotal))
        {
            AnalyticsService.Instance.CustomData("NumberOfBattlesTotal", new Dictionary<string, object> { { "numberOfBattlesTotal", _analyticsStorageSO.NumberOfBattlesTotal } });
        }
    }

    private void NumberOfBattlesWorlds(string worldSceneName)
    {
        if (worldSceneName == "World1Scene")
        {
            _analyticsStorageSO.NumberOfBattlesWorld1++;
            if (MustBeSent(_analyticsStorageSO.NumberOfBattlesWorld1))
            {
                AnalyticsService.Instance.CustomData("NumberOfBattlesWorlds", new Dictionary<string, object> { { "numberOfBattlesWorld1", _analyticsStorageSO.NumberOfBattlesWorld1 } });
            }
        }
        if (worldSceneName == "World1v2Scene")
        {
            _analyticsStorageSO.NumberOfBattlesWorld1v2++;
            if (MustBeSent(_analyticsStorageSO.NumberOfBattlesWorld1v2))
            {
                AnalyticsService.Instance.CustomData("NumberOfBattlesWorlds", new Dictionary<string, object> { { "numberOfBattlesWorld1v2", _analyticsStorageSO.NumberOfBattlesWorld1v2 } });
            }
        }
        if (worldSceneName == "World2Scene")
        {
            _analyticsStorageSO.NumberOfBattlesWorld2++;
            if (MustBeSent(_analyticsStorageSO.NumberOfBattlesWorld2))
            {
                AnalyticsService.Instance.CustomData("NumberOfBattlesWorlds", new Dictionary<string, object> { { "numberOfBattlesWorld2", _analyticsStorageSO.NumberOfBattlesWorld2 } });
            }
        }        
    }

    public void SendWorldFinishInfo(string worldName, string worldSOName, string missionStatus, int gameDifficulty)
    {
        if (missionStatus == "Completed")
        {
            if (worldName == "World1Scene")
                _analyticsStorageSO.MissionWorld1Completed++;
            if (worldName == "World1v2Scene")
                _analyticsStorageSO.MissionWorld1v2Completed++;
            if (worldName == "World2Scene")
                _analyticsStorageSO.MissionWorld2Completed++;
        }
        else
        {
            if (worldName == "World1Scene")
                _analyticsStorageSO.MissionWorld1Failed++;
            if (worldName == "World1v2Scene")
                _analyticsStorageSO.MissionWorld1v2Failed++;
            if (worldName == "World2Scene")
                _analyticsStorageSO.MissionWorld2Failed++;
        }
        AnalyticsService.Instance.CustomData("WorldFinishInfo", new Dictionary<string, object>
        {
            { "worldName", worldName },
            { "worldSOName", worldSOName },
            { "missionStatus", missionStatus },
            { "gameDifficulty", gameDifficulty }
        });
        AnalyticsService.Instance.CustomData("TotalMissionStatus", new Dictionary<string, object> { { "missionStatus", missionStatus } });
    }

    public void SendMissionTypeStatus(string missionType, int gameDifficulty, string missionStatus)
    {
        AnalyticsService.Instance.CustomData("MissionTypeStatus", new Dictionary<string, object>
        {
            { "missionType", missionType },
            { "gameDifficulty", gameDifficulty },
            { "missionStatus", missionStatus }
        });
    }

    public void SendMissionParamsStatus(string missionParams, int gameDifficulty, string missionStatus)
    {
        AnalyticsService.Instance.CustomData("MissionParamsStatus", new Dictionary<string, object>
        {
            { "missionParams", missionParams },
            { "gameDifficulty", gameDifficulty },
            { "missionStatus", missionStatus }
        });
        AnalyticsService.Instance.Flush();
    }

    public void SendNewPlayerLevel(int level)
    {
        AnalyticsService.Instance.CustomData("PlayerLevel", new Dictionary<string, object> { { "playerLevel", level } });
        AnalyticsService.Instance.Flush();
    }

    public void SendNumberOfDeaths()
    {
        _analyticsStorageSO.NumberOfDeaths++;
        if (MustBeSent(_analyticsStorageSO.NumberOfDeaths))
        {
            AnalyticsService.Instance.CustomData("PlayerNumberOfDeaths", new Dictionary<string, object>
            {
                { "numberOfDeaths", _analyticsStorageSO.NumberOfDeaths },
                { "playerLevel", _analyticsStorageSO.PlayerLevel }
            });
            AnalyticsService.Instance.Flush();
        }
    }

}
