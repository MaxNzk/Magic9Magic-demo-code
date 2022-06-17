using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerWorld2 : MonoBehaviour, IPassedThroughPortal, IPortal, IFinishMission
{
    public static event Action OnPause;
    public static event Action OnUnPause;
    [SerializeField] private GameObject _player;
    private FPCPlayerWeapons _playerWeaponsScripts;
    private PlayerPoints _playerPointsScript;
    private ManaMaker _manaMakerScript;
    private PlayerTakeDamage _playerTakeDamageScripts;

    [Space(10)]
    [Header("----- Mission Settings: ----------------------")]
    [SerializeField] private Missions.Type _missionType;
    [SerializeField] private Missions.Params _missionParams;
    [SerializeField] private float _cdTimerDefaultDuration = 180f;
    [SerializeField] private float _cdTimerMultiplier = 10f;
    private CountDownTimer _countDownTimer;
    private bool _isCountdownTimerMode;
    private bool _isManaLimitMode;
    private bool _isStealthMode;

    [Space(10)]
    [Header("----- Managers: ------------------------------")]
    [SerializeField] private StarterAssets.StarterAssetsInputs _inputManager;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private UIManagerPlayer _uiManagerPlayer;
    [SerializeField] private UIManagerWorld1 _uiManagerWorld;
    [SerializeField] private SpawnManagerType1 _spawnManager;
    [SerializeField] private ScriptableObjectPoolManager _poolManager;
    [SerializeField] private PopupTextPool _popupTextPoolManager;
    private ITimer _uiManagerWorldITimer;
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private GameAnalytics _analytics;

    [Space(10)]
    [Header("----- ScriptableObjects: ---------------------")]
    [SerializeField] private GameSettings _gameSettingsSO;
    [SerializeField] private PlayerSettings _playerSettingsSO;
    [SerializeField] private List<WorldDataSO> worldDataSOList = new List<WorldDataSO>();
    private Dictionary<string, WorldDataSO> worldDataSODictionary = new Dictionary<string, WorldDataSO>();

    [Space(10)]
    [Header("----- Others: --------------------------------")]
    [SerializeField] private LoadingScreen _loadingScreen;
    [SerializeField] private ToCampPortal _portal;
    private MyCVDFilter _cdvFilter;
    private IWorldGeneratable _worldGenerator;

    private void Awake()
    {
        Tools.Log("--------- World 2 -----------");
        SetMissionsParams();
        FindAndTestComponents();
        LoadSettings();
        Initialize();
    }

    private void SetMissionsParams()
    {
        _isCountdownTimerMode = (_missionParams & Missions.Params.CountdownTimerMode) != 0;
        _isManaLimitMode = (_missionParams & Missions.Params.ManaLimitMode) != 0;
        _isStealthMode = (_missionParams & Missions.Params.StealthMode) != 0;
    }

    private void FindAndTestComponents()
    {
        _cdvFilter = FindObjectOfType<MyCVDFilter>();
        _countDownTimer = GetComponent<CountDownTimer>();
        _worldGenerator = GetComponent<IWorldGeneratable>();

        _playerWeaponsScripts = _player.GetComponent<FPCPlayerWeapons>();
        _playerPointsScript = _player.GetComponent<PlayerPoints>();
        _manaMakerScript = _player.GetComponent<ManaMaker>();
        _playerTakeDamageScripts = _player.GetComponent<PlayerTakeDamage>();

        _uiManagerWorldITimer = (ITimer) _uiManagerWorld;

        Tools.CheckSingleInstance<GameManagerWorld2>(gameObject);
        if (_loadingScreen == null)
            Tools.LogError("LoadingScreen _loadingScreen = NULL");

        if (_player == null)
            Tools.LogError("GameObject _player = NULL");
        if (_gameSettingsSO == null)
            Tools.LogError("GameSettings _gameSettingsSO = NULL");
        if (_playerSettingsSO == null)
            Tools.LogError("PlayerSettings _playerSettingsSO = NULL");

        if (_cdvFilter == null)
            Tools.LogError("MyCVDFilter _cdvFilter = NULL");
        
        if (_inputManager == null)
            Tools.LogError("StarterAssets.StarterAssetsInputs _inputManager = NULL");
        if (_soundManager == null)
            Tools.LogError("SoundManager _soundManager = NULL");
        if (_uiManagerPlayer == null)
            Tools.LogError("UIManagerPlayer _uiManagerPlayer = NULL");
        if (_uiManagerWorld == null)
            Tools.LogError("UIManagerWorld1 _uiManagerWorld = NULL");
        if (_spawnManager == null)
            Tools.LogError("SpawnManagerType1 _spawnManager = NULL");
        if (_poolManager == null)
            Tools.LogError("ScriptableObjectPoolManager _poolManager = NULL");
        if (_popupTextPoolManager == null)
            Tools.LogError("PopupTextPool _popupTextPoolManager = NULL");
        if (_saveManager == null)
            Tools.LogError("SaveManager _saveManager = NULL");
        if (_analytics == null)
            Tools.LogError("GameAnalytics _analytics = NULL");

        if (_countDownTimer == null)
            Tools.LogError("CountDownTimer _countDownTimer = NULL");
        if (_worldGenerator == null)
            Tools.LogError("IWorldGeneratable _worldGenerator = NULL");
    }

    private void LoadSettings()
    {
        // LoadGraphicsQuality() - loading in MainScene or SettingsScene
        LoadColorBlindMode();     
    }

    private void LoadColorBlindMode()
    {
        _cdvFilter.SetCurrentColorType(_gameSettingsSO.ColorBlindType);
    }

    private void Initialize()
    {
        _loadingScreen.gameObject.SetActive(true);
        _loadingScreen.StartingScene();
        _poolManager.Initialize();
        _popupTextPoolManager.Initialize();

        ConvertWorldDataSOListToDic();
        _worldGenerator.CreateWorld(worldDataSODictionary[_gameSettingsSO.WorldSOName], _gameSettingsSO.GameDifficulty);

        _spawnManager.Initialize(_poolManager);
        float countdownTimerDuration;
        if (_missionType == Missions.Type.TimerType)
            countdownTimerDuration = _cdTimerDefaultDuration + _gameSettingsSO.GameDifficulty * _cdTimerMultiplier;
        else
            countdownTimerDuration = _cdTimerDefaultDuration - _gameSettingsSO.GameDifficulty * _cdTimerMultiplier;
        _uiManagerWorld.ShowSurviveXminutes();

        _analytics.Initialize();
        _analytics.TakeNextFirstStep(4);
        _inputManager.Initialize(_uiManagerPlayer);
        _portal.Initialize(gameManager: this, _analytics, _soundManager);
        _portal.Hide();
        _saveManager.Initialize(isFirstStartEver: false);
        _playerWeaponsScripts.Initialize(_inputManager);
        _playerPointsScript.Initialize(_uiManagerPlayer);
        _playerTakeDamageScripts.Initialize(gameManagerIFinishMission: this, _uiManagerPlayer);
        _manaMakerScript.Initialize(_uiManagerPlayer, isManaLimitMode: false);

        if (_isCountdownTimerMode)
            _countDownTimer.Initialize(gameManager: this, countdownTimerDuration, _uiManagerWorldITimer);
        else
            _uiManagerWorld.HideTimer();

        if (_isManaLimitMode)
            _manaMakerScript.Initialize(_uiManagerPlayer, isManaLimitMode: true);
        else
            _manaMakerScript.Initialize(_uiManagerPlayer, isManaLimitMode: false);
        
        _spawnManager.StartSpawner();
    }

    private void ConvertWorldDataSOListToDic()
    {
        for(int i = 0; i < worldDataSOList.Count; i++)
            worldDataSODictionary.Add(worldDataSOList[i].name, worldDataSOList[i]);
    }

    public void PassedThroughPortal(string worldSceneName, string worldSOName)
    {
        NextScene();
    }

    public void ShowPortal()
    {
        _portal.Show();
    }

    public void ShowPortal(Vector3 position)
    {
        _portal.Show(position);
    }

    private void NextScene()
    {
        _loadingScreen.FinishingScene();
        StartCoroutine(LoadSceneWithDelay(3.0f));
    }

    private IEnumerator LoadSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds (delay);
        AsyncOperation async = SceneManager.LoadSceneAsync("CampScene");
		yield return async;
    }

    public void Pause()
    {
        OnPause?.Invoke();
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        OnUnPause?.Invoke();
        Time.timeScale = 1;
    }

    
    public void FinishMission(Missions.Status missionStatus)
    {
        if (_missionType == Missions.Type.TimerType)
        {
            if (missionStatus == Missions.Status.TimeOver)
                FinishMissionCompleted();
            if (_isCountdownTimerMode)
            {
                if (missionStatus == Missions.Status.PlayerDied)
                    FinishMissionFailed();
            }
            if (_isManaLimitMode)
            {
                if (missionStatus == Missions.Status.PlayerDied)
                    FinishMissionFailed();
            }
            if (_isStealthMode)
            {
                if (missionStatus == Missions.Status.PlayerDied)
                    FinishMissionFailed();
                if (missionStatus == Missions.Status.AlarmTriggered)
                    FinishMissionFailed();
            }
        }  
    }

    private void FinishMissionCompleted()
    {
        _uiManagerWorld.ShowMissionCompleted();
        _spawnManager.StopSpawner();
        StopAllEnemies();
        _analytics.SendWorldFinishInfo("World2", _gameSettingsSO.WorldSOName, "Completed", _gameSettingsSO.GameDifficulty);
        _analytics.SendMissionTypeStatus(_missionType.ToString(), _gameSettingsSO.GameDifficulty, "Completed");
        _analytics.SendMissionParamsStatus(ParamsToString(), _gameSettingsSO.GameDifficulty, "Completed");
        StartCoroutine(WaitEndMessage());
    }

    private void FinishMissionFailed()
    {
        _uiManagerWorld.ShowMissionFailed();
        _spawnManager.StopSpawner();
        StopAllEnemies();
        _analytics.SendWorldFinishInfo("World2", _gameSettingsSO.WorldSOName, "Failed", _gameSettingsSO.GameDifficulty);
        _analytics.SendMissionTypeStatus(_missionType.ToString(), _gameSettingsSO.GameDifficulty, "Failed");
        _analytics.SendMissionParamsStatus(ParamsToString(), _gameSettingsSO.GameDifficulty, "Failed");
        StartCoroutine(WaitEndMessage());
    }

    private void StopAllEnemies()
    {
        var allEnemies = FindObjectsOfType<EnemyMovement>();
        foreach (var movementScript in allEnemies)
            movementScript.Stop();
    }

    private IEnumerator WaitEndMessage()
    {
        yield return new WaitForSeconds(2.0f);
        ShowPortal(_playerTakeDamageScripts.transform.position);
    }

    private string ParamsToString()
    {
        string result = "";
        if ((_missionParams & Missions.Params.CountdownTimerMode) != 0) result += "Timer";
        if ((_missionParams & Missions.Params.ManaLimitMode) != 0) result += "Mana";
        if ((_missionParams & Missions.Params.StealthMode) != 0) result += "Stealth";
        if (String.IsNullOrEmpty(result))
            result = "None";
        else
            result += "Mode";
        return result;
    }

}
