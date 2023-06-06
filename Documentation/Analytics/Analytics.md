# Analytics

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)

**Analytics:**

Unity Analytics Analytics local data is stored in ScriptableObject (Assets > Features > Analytics > Data > AnalyticsStorage.asset). For storing analytics data between gaming sessions.

![Analytics 1](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/Documentation/Analytics/analytics01.jpg?raw=true)

  IsMustBeSent: num < 10; == 20; 50; 100; 200; 500; 1000

-------------------------------------------------------------------------------------------

  **Method name**

  Event name : params (type)  [IsMustBeSent: True/False ]

-------------------------------------------------------------------------------------------

  **public void StartNewGameSession()**

    GameSession : gameSessionNum (int) [IsMustBeSent: True]

-------------------------------------------------------------------------------------------

  **public void TakeNextFirstStep(int stepNumber)**

    FirstPath : step (int)  [IsMustBeSent: False]

-------------------------------------------------------------------------------------------

  **public void ChangeColorBlindType(string colorType)**

    ColorBlindType : colorBlindType (string)  [IsMustBeSent: False]

-------------------------------------------------------------------------------------------

  **public void PassedThroughPortal(string worldSceneName, string worldSOName)**

    PassedThroughPortal : worldSceneName (string), worldSOName (string)  [IsMustBeSent: True]

    NumberOfBattlesTotal : numberOfBattlesTotal (int)  [IsMustBeSent: True]

    NumberOfBattlesWorlds : numberOfBattlesWorld1, numberOfBattlesWorld1v2, numberOfBattlesWorld2 (int)  [IsMustBeSent: True]

-------------------------------------------------------------------------------------------

  **public void SendWorldFinishInfo(string worldName, string worldSOName, string missionStatus, int gameDifficulty)**
    
    WorldFinishInfo : worldName, worldSOName, missionStatus, gameDifficulty  [IsMustBeSent: False]

    TotalMissionStatus : missionStatus (string)  [IsMustBeSent: False]

-------------------------------------------------------------------------------------------

  **public void SendMissionTypeStatus(string missionType, int gameDifficulty, string missionStatus)**

    MissionTypeStatus : missionType, gameDifficulty, missionStatus  [IsMustBeSent: False]

-------------------------------------------------------------------------------------------

  **public void SendMissionParamsStatus(string missionParams, int gameDifficulty, string missionStatus)**

    MissionParamsStatus : missionParams, gameDifficulty, missionStatus  [IsMustBeSent: False]

-------------------------------------------------------------------------------------------

  **public void SendNewPlayerLevel(int level)**

    PlayerLevel : playerLevel (int)  [IsMustBeSent: False]

-------------------------------------------------------------------------------------------

  **public void SendNumberOfDeaths()**

      PlayerNumberOfDeaths : numberOfDeaths, playerLevel (int)  [IsMustBeSent: True]

-------------------------------------------------------------------------------------------

[Back to README](https://github.com/MaxNzk/Magic9Magic-demo-code/blob/main/README.md)
