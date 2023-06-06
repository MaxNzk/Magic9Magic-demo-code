using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerCamp : MonoBehaviour, IPassedThroughPortal, IManaMaker
{
    [SerializeField] private GameObject _player;
    private PlayerMovement _playerMovementScript;
    private PlayerWeapons _playerWeaponsScripts;
    private PlayerPoints _playerPointsScript;
    private FootstepSounds _playerFootstepSoundsScripts;
    private PlayerTakeDamage _playerTakeDamageScripts;
    private CharacterStats _characterStats;

    private ManaMaker _manaMakerScript;
    public bool IsManaLimitMode { get; private set; }
    public int MaxMissionMana { get; private set; }

    [Header("----- Managers: ------------------------------")]
    [SerializeField] private Scenes _scenes;
    [SerializeField] private InputManager _inputManager;

    [SerializeField] private UIManager _uiManagerPlayer;
    private IUIManagerPlayer _IUIManagerPlayer;
    private IUIManagerInventory _IUIManagerInventory;
    private IUIManagerInput _IUIManagerInput;
    private IUIManagerWallet _IUIManagerWallet;
    private IUIManagerBloodyscreen _IUIManagerBloodyscreen;

    [SerializeField] private UIManager _uiManagerCamp;
    private IUIManagerMenu _IUIManagerMenu;
    private IUIManagerMarket _IUIManagerMarket;
    private IUIManagerPortal _IUIManagerPortal;
    private IUIManagerTutorialInput _IUIManagerTutorialInput;

    [SerializeField] private DialogueNotInteractive _dialogueNotInteractive;
    [SerializeField] private MagicPoolManager _magicPoolManager;
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private Wallet _wallet;

    [SerializeField] private MenuManager _menuManager;
    private MenuCampManager _menuCampManager;

    [SerializeField] private CutscenePlayer _cutscenePlayer;
    private CSP_CampFirst _cutsceneFirst;

    [SerializeField] private SoundManager _soundManagerGO;
    private ISoundManager _soundManager;

    [SerializeField] private LootManager _lootManagerGO;
    private ILootManager _lootManager;

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
    [Header("----- Portals: -------------------------------")]
    [SerializeField] private Portal _portal0;
    [SerializeField] private Portal _portal1;
    [SerializeField] private Portal _portal2;
    [SerializeField] private float _waitTeloportingTime = 2.0f;

    [Space(10)]
    [Header("----- Others: --------------------------------")]
    [SerializeField] private CVDFilter _cdvFilter;
    [SerializeField] private UpdatesPopup _updatesPopup;
    [SerializeField] private MarketTrigger _marketTrigger;

    private const string _lootFirstInitName = "FirstInit";

    private void Awake()
    {
        FindAndTestComponents();

        PreInitialize();
        Initialize();
        PostInitialize();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckSingleInstance<GameManagerCamp>(gameObject);

        Tools.CheckNull<GameObject>(_player, nameof(_player), gameObject);
        _playerMovementScript = Tools.GetComponentWithAssertion<PlayerMovement>(_player);
        _playerWeaponsScripts = Tools.GetComponentWithAssertion<PlayerWeapons>(_player);
        _playerPointsScript = Tools.GetComponentWithAssertion<PlayerPoints>(_player);
        _playerFootstepSoundsScripts = Tools.GetComponentInChildrenWithAssertion<FootstepSounds>(_player);
        _playerTakeDamageScripts = Tools.GetComponentWithAssertion<PlayerTakeDamage>(_player);
        _characterStats = Tools.GetComponentWithAssertion<CharacterStats>(_player);

        Tools.CheckNull<UIManager>(_uiManagerPlayer, nameof(_uiManagerPlayer), gameObject);
        _IUIManagerPlayer = Tools.GetComponentWithAssertion<IUIManagerPlayer>(_uiManagerPlayer.gameObject);
        _IUIManagerInventory = Tools.GetComponentWithAssertion<IUIManagerInventory>(_uiManagerPlayer.gameObject);
        _IUIManagerInput = Tools.GetComponentWithAssertion<IUIManagerInput>(_uiManagerPlayer.gameObject);
        _IUIManagerWallet = Tools.GetComponentWithAssertion<IUIManagerWallet>(_uiManagerPlayer.gameObject);
        _IUIManagerBloodyscreen = Tools.GetComponentWithAssertion<IUIManagerBloodyscreen>(_uiManagerPlayer.gameObject);

        Tools.CheckNull<UIManager>(_uiManagerCamp, nameof(_uiManagerCamp), gameObject);
        _IUIManagerMenu = Tools.GetComponentWithAssertion<IUIManagerMenu>(_uiManagerCamp.gameObject);
        _IUIManagerMarket = Tools.GetComponentWithAssertion<IUIManagerMarket>(_uiManagerCamp.gameObject);
        _IUIManagerPortal = Tools.GetComponentWithAssertion<IUIManagerPortal>(_uiManagerCamp.gameObject);
        _IUIManagerTutorialInput = Tools.GetComponentWithAssertion<IUIManagerTutorialInput>(_uiManagerCamp.gameObject);

        Tools.CheckNull<Scenes>(_scenes, nameof(_scenes), gameObject);
        Tools.CheckNull<InputManager>(_inputManager, nameof(_inputManager), gameObject);
        Tools.CheckNull<DialogueNotInteractive>(_dialogueNotInteractive, nameof(_dialogueNotInteractive), gameObject);
        Tools.CheckNull<MagicPoolManager>(_magicPoolManager, nameof(_magicPoolManager), gameObject);
        Tools.CheckNull<SaveManager>(_saveManager, nameof(_saveManager), gameObject);

        Tools.CheckNull<MenuManager>(_menuManager, nameof(_menuManager), gameObject);
        _menuCampManager = Tools.GetComponentWithAssertion<MenuCampManager>(_menuManager.gameObject);

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

        _wallet = Tools.GetComponentWithAssertion<Wallet>(_player);
        _manaMakerScript = Tools.GetComponentWithAssertion<ManaMaker>(_player);
            
        Tools.CheckNull<GameSettingsSO>(_gameSettingsSO, nameof(_gameSettingsSO), gameObject);
        Tools.CheckNull<GameStateSettingsSO>(_gameStateSettingsSO, nameof(_gameStateSettingsSO), gameObject);
        Tools.CheckNull<PlayerSettings>(_playerSettingsSO, nameof(_playerSettingsSO), gameObject);
        Tools.CheckNull<AnalyticsStorage>(_analyticsStorageSO, nameof(_analyticsStorageSO), gameObject);
        Tools.CheckNull<CutsceneSettingsSO>(_cutsceneSettingsSO, nameof(_cutsceneSettingsSO), gameObject);
        Tools.CheckNull<DialogueSettingsSO>(_dialogueSettingsSO, nameof(_dialogueSettingsSO), gameObject);

        Tools.CheckNull<CutscenePlayer>(_cutscenePlayer, nameof(_cutscenePlayer), gameObject);
        _cutsceneFirst = Tools.GetComponentWithAssertion<CSP_CampFirst>(_cutscenePlayer.gameObject);

        Tools.CheckNull<CVDFilter>(_cdvFilter, nameof(_cdvFilter), gameObject);
        Tools.CheckNull<UpdatesPopup>(_updatesPopup, nameof(_updatesPopup), gameObject);
        Tools.CheckNull<MarketTrigger>(_marketTrigger, nameof(_marketTrigger), gameObject);

        Tools.CheckNull<Portal>(_portal0, nameof(_portal0), gameObject);
        Tools.CheckNull<Portal>(_portal1, nameof(_portal1), gameObject);
        Tools.CheckNull<Portal>(_portal2, nameof(_portal2), gameObject);
    }

    private void PreInitialize()
    {
        _cdvFilter.SetCurrentColorType(_gameSettingsSO.ColorBlindType);
        Lean.Localization.LeanLocalization.SetCurrentLanguageAll(_gameSettingsSO.Language);
    }

    private void Initialize()
    {
        _scenes.Initialize();
        _analytics.Initialize(_analyticsStorageSO);
        _saveManager.Initialize();
        _inputManager.Initialize(_IUIManagerInput, _IUIManagerMenu);
        _soundManager.Initialize();
        _magicPoolManager.Initialize(_soundManager);
        _lootManager.Initialize(_gameStateSettingsSO, _playerSettingsSO);
        _inputManager.Activate();
        _marketTrigger.Initialize(_IUIManagerMarket, _IUIManagerInventory);

        _menuCampManager.Initialize(_IUIManagerMenu, _soundManager);
        _menuCampManager.Initialize(_scenes);

        _wallet.Initialize(_playerSettingsSO, _IUIManagerWallet);
        _portal0.Initialize(this, _gameStateSettingsSO, _analytics, _soundManager, _IUIManagerPortal, _wallet);
        _portal1.Initialize(this, _gameStateSettingsSO, _analytics, _soundManager, _IUIManagerPortal, _wallet);
        _portal2.Initialize(this, _gameStateSettingsSO, _analytics, _soundManager, _IUIManagerPortal, _wallet);

        _playerMovementScript.Initialize(_inputManager);
        _playerWeaponsScripts.Initialize(_magicPoolManager, _soundManager, _inputManager);
        _playerPointsScript.Initialize(_playerSettingsSO, _IUIManagerPlayer, _analytics);

        _manaMakerScript.Initialize(this, _playerSettingsSO, _soundManager, _IUIManagerPlayer);
        _playerTakeDamageScripts.Initialize(_playerSettingsSO, _dialogueSettingsSO, gameManagerIFinishMission: null, _soundManager, _IUIManagerPlayer, _IUIManagerBloodyscreen);
        _playerFootstepSoundsScripts.Initialize(_soundManager);
        _characterStats.Initialize(_playerSettingsSO, _soundManager, _IUIManagerInventory, _IUIManagerMarket);

        _dialogueNotInteractive.Initialize(_soundManager);
        _updatesPopup.Initialize(_gameSettingsSO);

        _cutsceneFirst.Initialize(_cutsceneSettingsSO, _inputManager);
    }

    private void PostInitialize()
    {
        _wallet.UpdateUI();
        _IUIManagerPlayer.SetPlayerName(_playerSettingsSO.playerName);
        StartCoroutine(SetLanguageWithDelay());
    }

    private IEnumerator SetLanguageWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        Lean.Localization.LeanLocalization.SetCurrentLanguageAll(_gameSettingsSO.Language);
    }

    public void PassedThroughPortal(string worldSceneName, string worldSOName, int portalNumber)
    {
        _inputManager.Deactivate();

        _gameStateSettingsSO.WorldSOName = worldSOName;

        _analyticsStorageSO.ThroughPortalsAmount[portalNumber] += 1;
        _analytics.TakeNextFirstStep(3);
        _analytics.TakeNextFirstStep(7);
        _analytics.TakeNextFirstStep(11);
        _analytics.TakeNextFirstStep(15);

        _scenes.Load(worldSceneName, _waitTeloportingTime);
    }
    
    //-----------------------------------------------------------------------------------
    //-------------- V I S U A L   S T A T E   M A C H I N E ----------------------------
    
    //>>> Portals >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    public void StartWorkingPortal0() => _portal0.StartWorking();
    public void StopWorkingPortal0() => _portal0.StopWorking();
    public void StartWorkingPortal1() => _portal1.StartWorking();
    public void StopWorkingPortal1() => _portal1.StopWorking();
    public void StartWorkingPortal2() => _portal2.StartWorking();
    public void StopWorkingPortal2() => _portal2.StopWorking();

    public bool ToStartWorkingPorta0()
    {
        if (_analyticsStorageSO.ThroughPortalsAmount[0] == 0 &&
            _analyticsStorageSO.ThroughPortalsAmount[1] == 0 &&
            _analyticsStorageSO.ThroughPortalsAmount[2] == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ToStartWorkingPorta1()
    {
        if (_analyticsStorageSO.ThroughPortalsAmount[0] > 0 &&
            _analyticsStorageSO.ThroughPortalsAmount[1] == 0 &&
            _analyticsStorageSO.ThroughPortalsAmount[2] == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ToStartWorkingPorta2()
    {
        if (_analyticsStorageSO.ThroughPortalsAmount[0] > 0 &&
            _analyticsStorageSO.ThroughPortalsAmount[1] > 0 &&
            _analyticsStorageSO.ThroughPortalsAmount[2] == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public bool ToStartWorkingPorta012()
    {
        if (_analyticsStorageSO.ThroughPortalsAmount[0] > 0 &&
            _analyticsStorageSO.ThroughPortalsAmount[1] > 0 &&
            _analyticsStorageSO.ThroughPortalsAmount[2] > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //>>> Cutscenes >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    public bool IsCutsceneStopped() => _cutscenePlayer.IsCutsceneStopped;
    public bool ToCutsceneFirst() => _cutsceneFirst.ToCutscene();
    public void PlayCutsceneFirst() => _cutsceneFirst.Play();

    //>>> Tutorials >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    public bool ToShowTutorialInput1() => _gameSettingsSO.IsTutorialWorld1Input1 == false;
    public void ShowTutorialInput1()
    {
        _gameSettingsSO.IsTutorialWorld1Input1 = true;
        _IUIManagerTutorialInput.ShowTutorialInput();
    }
    //>>> Different >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    public bool ToCheckForUpdates() => _updatesPopup.IsCheckForUpdates();
    public void CheckForUpdates() => _updatesPopup.Check(_gameSettingsSO.CurrentVersion);

    public void AnalyticsTakeNextStep()
    {
        _analytics.TakeNextFirstStep(2);
        _analytics.TakeNextFirstStep(6);
        _analytics.TakeNextFirstStep(10);
        _analytics.TakeNextFirstStep(14);
    }

    public bool ToInitLoot() => _gameSettingsSO.IsLootCampInit == false;
    public void GetLootInit()
    {
        _gameSettingsSO.IsLootCampInit = true;
        _lootManager.Get(isSenderPosition: false, Vector3.zero, _lootFirstInitName, isSenderItemListsSOIndex: false, 0, 1, 1, 0f, 1.3f);
    }
    //>>> Dialogues >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    public bool ToDialogueFirstDialogue() => _dialogueSettingsSO.IsFirstDialogue == false;
    public void StartFirstDialogue()
    {
        _dialogueSettingsSO.IsFirstDialogue = true;
        _dialogueNotInteractive.StartDialogue(DialogueNotInteractive.DialogueNames.FirstCamp);
    }

    public bool ToDialogueFirstDeath() => (_dialogueSettingsSO.IsFirstDeath == true) && (_dialogueSettingsSO.IsFirstDeathPlayed == false);
    public void StartFirstDeath()
    {
        _dialogueSettingsSO.IsFirstDeathPlayed = true;
        _dialogueNotInteractive.StartDialogue(DialogueNotInteractive.DialogueNames.FirstDeath);
    }

    public bool ToDialoguePickUpFirstItem() => _dialogueSettingsSO.IsPickUpFirstItem == false;
    public void StartDialoguePickUpFirstItem()
    {
        _dialogueSettingsSO.IsPickUpFirstItem = true;
        _dialogueNotInteractive.StartDialogue(DialogueNotInteractive.DialogueNames.PickUpFirstItem);
    }

}
