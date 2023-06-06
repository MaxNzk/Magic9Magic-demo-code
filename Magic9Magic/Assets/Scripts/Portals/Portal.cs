using UnityEngine;

public class Portal : MonoBehaviour
{   
    [SerializeField] private int _portalNumber;
    [SerializeField] private PortalPopup _portalPopup;
    [SerializeField] private PortalDataSO _portalDataSO;
    [SerializeField] private PortalWorldDescriptionSO _portalWorldDescriptionSO;
    [SerializeField] private WorldDataSO _worldDataSO;
    [SerializeField] private SoundManager.SoundNamesUI soundNameAppearing;
    [SerializeField] private SoundManager.SoundNamesUI soundNameSending;

    [Space(10)]
    [SerializeField] private Renderer _platformRenderer;
    [SerializeField] private GameObject _workingEffect;
    [SerializeField] private GameObject _portingEffect;

    private IPassedThroughPortal _gameManager;
    private GameStateSettingsSO _gameStateSettingsSO;
    private IAnalytics _analytics;
    private ISoundManager _soundManager;
    private IUIManagerPortal _uiManager;
    private Wallet _wallet;

    private bool _isInitialized;
    private bool _isWorking;
    private IPortalable _portalable;

    private Lean.Localization.LeanLocalizedSO _localization;

    private void Awake()
    {
        FindAndTestComponents();
        SetPlatformMaterial();
    }

    public void Initialize(IPassedThroughPortal gameManager, GameStateSettingsSO gameStateSettingsSO, IAnalytics analytics, ISoundManager soundManager, IUIManagerPortal uiManager, Wallet wallet)
    {
        if (_isInitialized)
            return;

        _gameManager = gameManager;
        _gameStateSettingsSO = gameStateSettingsSO;
        _analytics = analytics;
        _soundManager = soundManager;
        _uiManager = uiManager;
        _wallet = wallet;

        _isInitialized = true;
    }

    public void StartWorking()
    {
        _isWorking = true;
        _workingEffect.SetActive(true);
    }

    public void StopWorking()
    {
        _isWorking = false;
        _workingEffect.SetActive(false);
    }

    private void FindAndTestComponents()
    {
        Tools.CheckNull<PortalPopup>(_portalPopup, nameof(_portalPopup), gameObject);
        Tools.CheckNull<PortalDataSO>(_portalDataSO, nameof(_portalDataSO), gameObject);
        Tools.CheckNull<PortalWorldDescriptionSO>(_portalWorldDescriptionSO, nameof(_portalWorldDescriptionSO), gameObject);
        Tools.CheckNull<WorldDataSO>(_worldDataSO, nameof(_worldDataSO), gameObject);

        Tools.CheckNull<Renderer>(_platformRenderer, nameof(_platformRenderer), gameObject);
        
        if (string.IsNullOrEmpty(_portalDataSO.SceneName))
            Tools.LogError("Setup portal world snene name!");

        _localization = Tools.GetComponentWithAssertion<Lean.Localization.LeanLocalizedSO>(gameObject);
    }

    private void SetPlatformMaterial()
    {
        _platformRenderer.material = _portalDataSO.PortalMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isWorking == false)
            return;

        if (other.TryGetComponent<IPortalable>(out IPortalable portalable))
        {
            _portalable = portalable;
            _portalable.SetDestinationAtPoint(transform.position);

            _portalWorldDescriptionSO = (PortalWorldDescriptionSO) _localization.LocalizedSO;
            
            _uiManager.SetWorldName(_portalWorldDescriptionSO.WorldName);
            _uiManager.SetWorldDescription(_portalWorldDescriptionSO.WorldDescription);
            _uiManager.SetClosePrice(_portalDataSO.ClosePrice.ToString());
            _uiManager.SetMiddlePrice(_portalDataSO.MiddlePrice.ToString());
            _uiManager.SetFarPrice(_portalDataSO.FarPrice.ToString());
            _uiManager.ShowPortal();

            _portalPopup.ActivePortal = this;
        }
    }

    public void Show()
    {
        _soundManager.Play(soundNameAppearing.ToString());
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ChooseDistance(int variant)
    {
        if (variant < 0 || variant > 2)
            Tools.LogError("Portal: Choose distance (int variant) is wrong");

        _gameStateSettingsSO.GameDifficulty = variant;

        int price = 0;
        if (variant == 0)
        {
            price = _portalDataSO.ClosePrice;
        }
        if (variant == 1)
        {
            price = _portalDataSO.MiddlePrice;
        }
        if (variant == 2)
        {
            price = _portalDataSO.FarPrice;
        }

        if (_wallet.TryPayWithSoulStone(price))
        {
            Send();
        }
        else
        {
            _uiManager.ShowNotEnoughText();
        }
    }

    private void Send()
    {
        _uiManager.HidePortal();
        _portingEffect.SetActive(true);
        _soundManager.Play(soundNameSending.ToString());
        _analytics.PassedThroughPortal(_portalDataSO.SceneName, _worldDataSO.WorldSOName);
        _gameManager.PassedThroughPortal(_portalDataSO.SceneName, _worldDataSO.WorldSOName, _portalNumber);
        _portalable.Disappear();
    }

}
