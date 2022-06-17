using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class GameManagerCamp : MonoBehaviour, IPassedThroughPortal
{
    public static event Action OnPause;
    public static event Action OnUnPause;
    [SerializeField] private GameObject _player;
    private PlayerMovement _playerMovementScript;
    private PlayerWeapons _playerWeaponsScripts;
    private PlayerPoints _playerPointsScript;
    private FootstepSounds _playerFootstepSoundsScripts;
    private ManaMaker _manaMakerScript;
    private PlayerTakeDamage _playerTakeDamageScripts;
    [Header("----- Managers: ------------------------------")]
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private UIManagerPlayer _uiManagerPlayer;
    [SerializeField] private UIManagerCamp _uiManagerCamp;
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private GameAnalytics _analytics;
    [Space(10)]
    [Header("----- ScriptableObjects: ---------------------")]
    [SerializeField] private GameSettings _gameSettingsSO;
    [Space(10)]
    [Header("----- Others: --------------------------------")]
    [SerializeField] private PlayableDirector _director;
    [SerializeField] private UpdatesPopup _updatesPopup;
    [SerializeField] private LoadingScreen _loadingScreen;
    [SerializeField] private NPCMarket _NPCMarket;
    [SerializeField] private Portal _portal1;
    [SerializeField] private Portal _portal2;
    private MyCVDFilter _cdvFilter;

    private void Awake()
    {
        Tools.Log("--------- Camp -----------");
        FindAndTestComponents();
        LoadSettings();
        Initialize();
    }

    private void FindAndTestComponents()
    { 
        _cdvFilter = FindObjectOfType<MyCVDFilter>();

        _playerMovementScript = _player.GetComponent<PlayerMovement>();
        _playerWeaponsScripts = _player.GetComponent<PlayerWeapons>();
        _playerPointsScript = _player.GetComponent<PlayerPoints>();
        _manaMakerScript = _player.GetComponent<ManaMaker>();
        _playerTakeDamageScripts = _player.GetComponent<PlayerTakeDamage>();
        _playerFootstepSoundsScripts = _player.GetComponentInChildren<FootstepSounds>();

        Tools.CheckSingleInstance<GameManagerCamp>(gameObject);
        if (_director == null)
            Tools.LogError("PlayableDirector _director = NULL");
        if (_updatesPopup == null)
            Tools.LogError("UpdatesPopup _updatesPopup = NULL");
        if (_cdvFilter == null)
            Tools.LogError("MyCVDFilter _cdvFilter = NULL");
        if (_player == null)
            Tools.LogError("GameObject _player = NULL");
        if (_playerFootstepSoundsScripts == null)
            Tools.LogError("FootstepSounds _playerFootstepSoundsScripts = NULL");
        if (_loadingScreen == null)
            Tools.LogError("LoadingScreen _loadingScreen = NULL");
        if (_inputManager == null)
            Tools.LogError("InputManager _inputManager = NULL");
        if (_soundManager == null)
            Tools.LogError("SoundManager _soundManager = NULL");
        if (_uiManagerPlayer == null)
            Tools.LogError("UIManagerPlayer _uiManagerPlayer = NULL");
        if (_uiManagerCamp == null)
            Tools.LogError("UIManagerCamp _uiManagerCamp = NULL");
        if (_saveManager == null)
            Tools.LogError("SaveManager _saveManager = NULL");
        if (_analytics == null)
            Tools.LogError("GameAnalytics _analytics = NULL");
    }

    private void LoadSettings()
    {
        LoadColorBlindMode();     
    }

    private void Initialize()
    {
        _analytics.Initialize();
        _analytics.TakeNextFirstStep(2);
        _saveManager.Initialize(isFirstStartEver: false);
        _inputManager.Initialize(_uiManagerPlayer);
        _inputManager.Activate();
        _loadingScreen.gameObject.SetActive(true);
        _loadingScreen.StartingScene();
        _NPCMarket.Initialize(_uiManagerCamp, _uiManagerPlayer);
        _portal1.Initialize(gameManager: this, _analytics, _soundManager);
        _portal2.Initialize(gameManager: this, _analytics, _soundManager);
        _playerMovementScript.Initialize(_inputManager);
        _playerWeaponsScripts.Initialize(_inputManager);
        _playerPointsScript.Initialize(_uiManagerPlayer);
        _manaMakerScript.Initialize(_uiManagerPlayer, isManaLimitMode: false);
        _playerTakeDamageScripts.Initialize(gameManagerIFinishMission: null, _uiManagerPlayer);
        _playerFootstepSoundsScripts.Initialize(_soundManager);
        if (CutsceneCampFirst() == false)
            if (_gameSettingsSO.IsCheckForUpdates == false)
            {
                _gameSettingsSO.IsCheckForUpdates = true;
                _updatesPopup.Check(_gameSettingsSO.CurrentVersion);
            }
    }

    private void CheckTutorial()
    {
        if (_analytics.IsCurrentFirstStep(3))
            _uiManagerCamp.ShowTutorialCanvasInput();
    }

    private bool CutsceneCampFirst()
    {
        if (_analytics.IsCurrentFirstStep(3))
        {
            _director.stopped += OnPlayableDirectorStopped;
            _inputManager.Deactivate();
            _director.Play();
            return true;
        }
        else
            return false;
    }

    private void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        _director.stopped -= OnPlayableDirectorStopped;
        _inputManager.Activate();
        CheckTutorial();
    }

    private void LoadColorBlindMode()
    {
        _cdvFilter.SetCurrentColorType(_gameSettingsSO.ColorBlindType);
    }

    public void PassedThroughPortal(string worldSceneName, string worldSOName)
    {
        _gameSettingsSO.WorldSOName = worldSOName;
        LoadWorldScene(worldSceneName);
    }

    private void LoadWorldScene(string worldSceneName)
    {
        _loadingScreen.FinishingScene();
        StartCoroutine(LoadScene(worldSceneName));
    }

    public void LoadSettingsScene()  // Canvas > Button_Circle01_Gray (Settings Button)
    {
        _loadingScreen.GoingToSettings();
        StartCoroutine(LoadScene("SettingsScene"));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        float waitTime = sceneName == "SettingsScene" ? 1.0f : 3.0f;
        yield return new WaitForSeconds (waitTime);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
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
    
}
