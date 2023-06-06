using System;
using System.Collections;
using UnityEngine;

public class GameManagerWorld1 : MonoBehaviour, IPassedThroughPortal, IPortal, IFinishMission, IManaMaker
{
    public static event Action OnFinishMission;
    
    [SerializeField] private GameObject _player;

    private PlayerMovement _playerMovementScript;
    private PlayerWeapons _playerWeaponsScripts;
    private PlayerPoints _playerPointsScript;
    private FootstepSounds _playerFootstepSoundsScripts;
    private PlayerTakeDamage _playerTakeDamageScripts;
    private CharacterStats _characterStats;
    private ManaMaker _manaMakerScript;

    [Space(10)]
    [Header("----- Mission Settings: ----------------------")]
    [SerializeField] private MissionConfigSO _missionConfigSO;
    private CountDownTimer _countDownTimer;
    private bool _isCountdownTimerMode;
    private bool _isStealthMode;
    public bool IsManaLimitMode { get; private set; }
    public int MaxMissionMana { get; private set; }
    [SerializeField] private ReachedTarget _reachedTarget;
    private bool _isStartMissionDescriptionHid;

    [Space(10)]
    [Header("----- Managers: ------------------------------")]
    [SerializeField] private Scenes _scenes;
    [SerializeField] private WorldCreator _worldGenerator;
    [SerializeField] private InputManager _inputManager;

    [SerializeField] private UIManager _uiManagerPlayer;
    private IUIManagerPlayer _IUIManagerPlayer;
    private IUIManagerInventory _IUIManagerInventory;
    private IUIManagerInput _IUIManagerInput;
    private IUIManagerWallet _IUIManagerWallet;
    private IUIManagerBloodyscreen _IUIManagerBloodyscreen;

    [SerializeField] private UIManager _uiManagerWorld;
    private IUIManagerMenu _IUIManagerMenu;
    private IUIManagerTutorialInput _IUIManagerTutorialInput;
    private IUIManagerTimer _IUIManagerTimer;
    private IUIManagerMission _IUIManagerMission;

    [Space(10)]
    [SerializeField] private EnemyPoolManager _enemyPoolManager;
    [SerializeField] private GameObject _spawnManagerGO;
    private ISpawnManager _spawnManager;
    [SerializeField] private GameObject _addressableManagerGO;
    private IAddressableManager _addressableManager;
    [Header("Or enemyFactory:")]
    [SerializeField] private EnemyFactory _enemyFactory;

    [Space(10)]
    [SerializeField] private LootManager _lootManagerGO;
    private ILootManager _lootManager;
    [SerializeField] private PopupTextPool _popupTextPoolManager;
    [SerializeField] private MagicPoolManager _magicPoolManager;
    [SerializeField] private SaveManager _saveManager;

    [SerializeField] private MenuManager _menuManager;
    private MenuWorldManager _menuWorldManager;

    [SerializeField] private SoundManager _soundManagerGO;
    private ISoundManager _soundManager;

    [SerializeField] private GameObject _analyticsGO;
    private IAnalytics _analytics;

    [Space(10)]
    [Header("----- ScriptableObjects: ---------------------")]
    [SerializeField] private GameSettingsSO _gameSettingsSO;
    [SerializeField] private GameStateSettingsSO _gameStateSettingsSO;
    [SerializeField] private PlayerSettings _playerSettingsSO;
    [SerializeField] private AnalyticsStorage _analyticsStorageSO;
    [SerializeField] private CutsceneSettingsSO _cutsceneSettingsSO;
    [SerializeField] private DialogueSettingsSO _dialogueSettingsSO;

    [Space(10)]
    [Header("----- Others: --------------------------------")]
    [SerializeField] private CVDFilter _cdvFilter;
    [SerializeField] private ToCampPortal _portal;

    private void Awake()
    {
        FindAndTestComponents();
        SetMissionsParams();

        PreInitialize();
        Initialize();
        PostInitialize();

        SetAnalyticsStepFirstWorld1();
        CreateWorld();
    }

    private void PrintSceneInfo()
    {
        Tools.Log("gameStateSettingsSO.GameDifficulty = " + _gameStateSettingsSO.GameDifficulty);
    }

    private void FindAndTestComponents()
    {
        Tools.CheckSingleInstance<GameManagerWorld1>(gameObject);

        Tools.CheckNull<GameObject>(_player, nameof(_player), gameObject);
        _playerMovementScript = Tools.GetComponentWithAssertion<PlayerMovement>(_player);
        _playerWeaponsScripts = Tools.GetComponentWithAssertion<PlayerWeapons>(_player);
        _playerPointsScript = Tools.GetComponentWithAssertion<PlayerPoints>(_player);
        _playerFootstepSoundsScripts = Tools.GetComponentInChildrenWithAssertion<FootstepSounds>(_player);
        _playerTakeDamageScripts = Tools.GetComponentWithAssertion<PlayerTakeDamage>(_player);
        _characterStats = Tools.GetComponentWithAssertion<CharacterStats>(_player);
        
        Tools.CheckNull<MissionConfigSO>(_missionConfigSO, nameof(_missionConfigSO), gameObject);
        _countDownTimer = Tools.GetComponentWithAssertion<CountDownTimer>(gameObject);

        Tools.CheckNull<UIManager>(_uiManagerPlayer, nameof(_uiManagerPlayer), gameObject);
        _IUIManagerPlayer = Tools.GetComponentWithAssertion<IUIManagerPlayer>(_uiManagerPlayer.gameObject);
        _IUIManagerInventory = Tools.GetComponentWithAssertion<IUIManagerInventory>(_uiManagerPlayer.gameObject);
        _IUIManagerInput = Tools.GetComponentWithAssertion<IUIManagerInput>(_uiManagerPlayer.gameObject);
        _IUIManagerWallet = Tools.GetComponentWithAssertion<IUIManagerWallet>(_uiManagerPlayer.gameObject);
        _IUIManagerBloodyscreen = Tools.GetComponentWithAssertion<IUIManagerBloodyscreen>(_uiManagerPlayer.gameObject);
        
        Tools.CheckNull<UIManager>(_uiManagerWorld, nameof(_uiManagerWorld), gameObject);
        _IUIManagerMenu = Tools.GetComponentWithAssertion<IUIManagerMenu>(_uiManagerWorld.gameObject);
        _IUIManagerTutorialInput = Tools.GetComponentWithAssertion<IUIManagerTutorialInput>(_uiManagerWorld.gameObject);
        _IUIManagerTimer = Tools.GetComponentWithAssertion<IUIManagerTimer>(_uiManagerWorld.gameObject);
        _IUIManagerMission = Tools.GetComponentWithAssertion<IUIManagerMission>(_uiManagerWorld.gameObject);

        Tools.CheckNull<Scenes>(_scenes, nameof(_scenes), gameObject);
        Tools.CheckNull<WorldCreator>(_worldGenerator, nameof(_worldGenerator), gameObject);
        Tools.CheckNull<InputManager>(_inputManager, nameof(_inputManager), gameObject);

        Tools.CheckNull<PopupTextPool>(_popupTextPoolManager, nameof(_popupTextPoolManager), gameObject);
        Tools.CheckNull<MagicPoolManager>(_magicPoolManager, nameof(_magicPoolManager), gameObject);

        Tools.CheckNull<SaveManager>(_saveManager, nameof(_saveManager), gameObject);
        
        Tools.CheckNull<MenuManager>(_menuManager, nameof(_menuManager), gameObject);
        _menuWorldManager = Tools.GetComponentWithAssertion<MenuWorldManager>(_menuManager.gameObject);

        if (_enemyFactory == null)
        {
            Tools.CheckNull<EnemyPoolManager>(_enemyPoolManager, nameof(_enemyPoolManager), gameObject);
            
            _addressableManager = _addressableManagerGO.GetComponent<IAddressableManager>();
            Tools.CheckNull<IAddressableManager>(_addressableManager, nameof(_addressableManager), gameObject);

            _spawnManager = _spawnManagerGO.GetComponent<ISpawnManager>();
            Tools.CheckNull<ISpawnManager>(_spawnManager, nameof(_spawnManager), gameObject);
        }

        _lootManager = _lootManagerGO.GetComponent<ILootManager>();
        Tools.CheckNull<ILootManager>(_lootManager, nameof(_lootManager), gameObject);

        _soundManager = _soundManagerGO.GetComponent<ISoundManager>();
        Tools.CheckNull<ISoundManager>(_soundManager, nameof(_soundManager), gameObject);

        _analytics = _analyticsGO.GetComponent<IAnalytics>();
        if (_analytics == null)
        {
            _analytics = FindObjectOfType<SingletonAnalytics>();
            Tools.CheckNull<IAnalytics>(_analytics, nameof(_analytics), gameObject);
        }

        _manaMakerScript = Tools.GetComponentWithAssertion<ManaMaker>(_player);

        Tools.CheckNull<GameSettingsSO>(_gameSettingsSO, nameof(_gameSettingsSO), gameObject);
        Tools.CheckNull<GameStateSettingsSO>(_gameStateSettingsSO, nameof(_gameStateSettingsSO), gameObject);
        Tools.CheckNull<PlayerSettings>(_playerSettingsSO, nameof(_playerSettingsSO), gameObject);
        Tools.CheckNull<AnalyticsStorage>(_analyticsStorageSO, nameof(_analyticsStorageSO), gameObject);
        Tools.CheckNull<CutsceneSettingsSO>(_cutsceneSettingsSO, nameof(_cutsceneSettingsSO), gameObject);
        Tools.CheckNull<DialogueSettingsSO>(_dialogueSettingsSO, nameof(_dialogueSettingsSO), gameObject);

        Tools.CheckNull<CVDFilter>(_cdvFilter, nameof(_cdvFilter), gameObject);
        Tools.CheckNull<ToCampPortal>(_portal, nameof(_portal), gameObject);

        if (_missionConfigSO.MissionType == Missions.Type.ReachTarget)
        {
            Tools.CheckNull<ReachedTarget>(_reachedTarget, nameof(_reachedTarget), gameObject);
        }
    }

    private void PreInitialize()
    {
        _cdvFilter.SetCurrentColorType(_gameSettingsSO.ColorBlindType);
        
        _IUIManagerTimer.HideTimer();
        _IUIManagerWallet.SetGold(_playerSettingsSO.Gold);
        _IUIManagerWallet.SetSoulStone(_playerSettingsSO.SoulStone);
    }

    private void Initialize()
    {
        _scenes.Initialize();
        _analytics.Initialize(_analyticsStorageSO);
        _lootManager.Initialize(_gameStateSettingsSO, _playerSettingsSO);
        _soundManager.Initialize();

        if (_enemyFactory != null)
            _enemyFactory.Initialize(enemySetIndex: _gameStateSettingsSO.GameDifficulty, _player.transform, _lootManager, _soundManager);
        if (_enemyPoolManager != null)
            _enemyPoolManager.Initialize(CalcWorldLevel(), _player.transform, _lootManager, _soundManager, _addressableManager);
        _popupTextPoolManager.Initialize();
        _magicPoolManager.Initialize(_soundManager);

        if (_spawnManager != null)
            _spawnManager.Initialize(_enemyPoolManager, _player.transform, CalcWorldLevel());
        
        _inputManager.Initialize(_IUIManagerInput, _IUIManagerMenu);

        _portal.Initialize(this, _analytics, _soundManager);
        if (_reachedTarget != null)
            _reachedTarget.Initialize(this);
        
        _saveManager.Initialize();
        
        _menuWorldManager.Initialize(_IUIManagerMenu, _soundManager);
        _menuWorldManager.Initialize(this);

        _playerMovementScript.Initialize(_inputManager);
        _playerWeaponsScripts.Initialize(_magicPoolManager, _soundManager, _inputManager);
        _playerPointsScript.Initialize(_playerSettingsSO, _IUIManagerPlayer, _analytics);
        _playerFootstepSoundsScripts.Initialize(_soundManager);
        _playerTakeDamageScripts.Initialize(_playerSettingsSO, _dialogueSettingsSO, gameManagerIFinishMission: this, _soundManager, _IUIManagerPlayer, _IUIManagerBloodyscreen);
        _characterStats.Initialize(_playerSettingsSO, _soundManager, _IUIManagerInventory, iUIManagerMarket: null);

        SetManaLimitMode();
        _manaMakerScript.Initialize(this, _playerSettingsSO, _soundManager, _IUIManagerPlayer);
    }

    private void PostInitialize()
    {
        PrintSceneInfo();
        StartMakeMana();
        SetCountdownTimerMode();
        StartCoroutine(SetLanguageWithDelay());
    }

    private int CalcWorldLevel()
    {
        if (_gameStateSettingsSO.GameDifficulty == 0) return Mathf.Clamp(_playerSettingsSO.Level - 2, 0, Int32.MaxValue);
        if (_gameStateSettingsSO.GameDifficulty == 1) return Mathf.Clamp(_playerSettingsSO.Level - 1, 0, Int32.MaxValue);
        if (_gameStateSettingsSO.GameDifficulty == 2) return Mathf.Clamp(_playerSettingsSO.Level, 0, Int32.MaxValue);

        return _playerSettingsSO.Level;
    }

    private IEnumerator SetLanguageWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        Lean.Localization.LeanLocalization.SetCurrentLanguageAll(_gameSettingsSO.Language);
    }

    private void SetMissionsParams()
    {
        _isCountdownTimerMode = (_missionConfigSO.MissionParams & Missions.Params.CountdownTimerMode) != 0;
        IsManaLimitMode = (_missionConfigSO.MissionParams & Missions.Params.ManaLimitMode) != 0;
        _isStealthMode = (_missionConfigSO.MissionParams & Missions.Params.StealthMode) != 0;
    }

    private void SetManaLimitMode()
    {
        if (IsManaLimitMode)
        {
            MaxMissionMana = _missionConfigSO.MaxMissionMana;
            MaxMissionMana -= _gameStateSettingsSO.GameDifficulty * 5;
        }
    }

    private void StartMakeMana()
    {
        if (IsManaLimitMode == false)
        {
            _manaMakerScript.StartMakeMana();
        }
    }

    private void SetCountdownTimerMode()
    {
        if (_isCountdownTimerMode)
        {
            float countdownTimerDuration;
            if (_missionConfigSO.MissionType == Missions.Type.TimerType)
            {
                countdownTimerDuration = _missionConfigSO.MissionDuration + _gameStateSettingsSO.GameDifficulty * _missionConfigSO.TimerDifficultyMultiplier;
            }
            else
            {
                countdownTimerDuration = _missionConfigSO.MissionDuration - _gameStateSettingsSO.GameDifficulty * _missionConfigSO.TimerDifficultyMultiplier;
            }
            _countDownTimer.Initialize(this, countdownTimerDuration, _IUIManagerTimer);
            _IUIManagerTimer.ShowTimer();
        }
    }

    private void SetAnalyticsStepFirstWorld1() => _analytics.TakeNextFirstStep(4);

    private void CreateWorld()
    {
        _worldGenerator.CreateWorld(_gameStateSettingsSO.WorldSOName, _gameStateSettingsSO.GameDifficulty);
    }

    public void ShowPortal(Vector3 position)
    {
        _portal.Show(position);
    }

    public void PassedThroughPortal(string worldSceneName, string worldSOName, int portalNumber)
    {
        _scenes.Load(worldSceneName, waitTeloportingTime: 0);
    }

    //-----------------------------------------------------------------------------------
    //-------------- M I S S I O N   R E S U L T ----------------------------------------

    public void FinishMission(Missions.Status missionStatus)
    {
        _analytics.TakeNextFirstStep(5);
        _analytics.TakeNextFirstStep(13);

        InvokeFinishMissionEvent();
        _inputManager.Deactivate();
        
        if (missionStatus != Missions.Status.ReachedTarget)
        {
            _playerMovementScript.StopAgent();
        }

        if (_missionConfigSO.FinishMissionSO.Finish(missionStatus))
        {
            FinishMissionCompleted();
        }
        else
        {
            FinishMissionFailed();
        }
    }

    private void InvokeFinishMissionEvent() => OnFinishMission?.Invoke();

    private void FinishMissionCompleted()
    {
        _IUIManagerMission.ShowMissionCompleted();
        _soundManager.Play(_missionConfigSO.SoundNameMissionCompleted.ToString());
        Finish("Completed");
    }

    private void FinishMissionFailed()
    {
        _IUIManagerMission.ShowMissionFailed();
        _soundManager.Play(_missionConfigSO.SoundNameMissionFailed.ToString());
        Finish("Failed");
    }

    private void Finish(string missionResult)
    {
        if (_spawnManager != null)
            _spawnManager.StopSpawner();

        _analytics.SendWorldFinishInfo(_scenes.CurrentSceneName, _gameStateSettingsSO.WorldSOName, missionResult, _gameStateSettingsSO.GameDifficulty);
        _analytics.SendMissionTypeStatus(_missionConfigSO.MissionType.ToString(), _gameStateSettingsSO.GameDifficulty, missionResult);
        _analytics.SendMissionParamsStatus(Missions.ParamsToString(_missionConfigSO.MissionParams), _gameStateSettingsSO.GameDifficulty, missionResult);
        
        StartCoroutine(WaitEndMessage());
    }

    private IEnumerator WaitEndMessage()
    {
        yield return new WaitForSeconds(2.0f);
        ShowPortal(_playerMovementScript.transform.position);
    }

    //-----------------------------------------------------------------------------------
    //-------------- V I S U A L   S T A T E   M A C H I N E ----------------------------

    public void ActivateInputManager() => _inputManager.Activate();
    public void DeactivateInputManager() => _inputManager.Deactivate();
    
    public void ShowMissionAssignment()
    {
        _IUIManagerMission.ShowMissionAssignment(_missionConfigSO.MissionAssignment.GetMissionAssignment(_gameSettingsSO.Language));
        StartCoroutine(HideMissionAssignment());
    }

    private IEnumerator HideMissionAssignment()
    {
        yield return new WaitForSeconds(_missionConfigSO.StartMissionDescriptionDuration);
        _isStartMissionDescriptionHid = true;
        _IUIManagerMission.HideMissionAssignment();
    }

    public bool IsMissionAssignmentHid() => _isStartMissionDescriptionHid;

    public void AnalyticsTakeNextStep()
    {
        _analytics.TakeNextFirstStep(4);
        _analytics.TakeNextFirstStep(12);
    }
    
    public void StartSpawner()
    {
        if (_spawnManager != null)
            _spawnManager.StartSpawner();
    }

}
