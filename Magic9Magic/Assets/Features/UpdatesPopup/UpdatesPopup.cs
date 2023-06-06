/*
The update is checked in CampScene once after each game launch.
GameManagerStart.cs Awake() >
                            private void SetDefaultGameSettings()
                            {
                                _gameSettingsSO.CurrentVersion = float.Parse(Application.version, CultureInfo.InvariantCulture.NumberFormat);
                                _gameSettingsSO.IsCheckForUpdates = false;
                            }
*/

using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;

public class UpdatesPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private Button _buttonUpdate;

    [Header("Button Later ---------------------------")]
    [SerializeField] private float _closeWaitDuration = 10.0f;
    [SerializeField] private Button _buttonLater;
    [SerializeField] private TextMeshProUGUI _textLater;
    private bool _isButtonLaterInteractable;
    
    [Space(10)]
    [Header("Remote Config ---------------------------")]
    [SerializeField] private bool _isDevelopment;
    [SerializeField] private string _DevEnvironmentID;
    private float _serverVersion;
    private float _currentVersion;
    private string _url;
    private Vector3 _mainTransformScale;
    private bool _isStart;
    private GameSettingsSO _gameSettingsSO;

    public void Initialize(GameSettingsSO gameSettingsSO)
    {
        _gameSettingsSO = gameSettingsSO;

        FindAndTestComponents();

        _buttonLater.interactable = false;
        _buttonUpdate.interactable = false;
    }

    private void FindAndTestComponents()
    {
        if (_closeWaitDuration <= 0)
            Tools.LogError("UpdatesPopup: _closeWaitDuration <= 0");

        Tools.CheckNull<Button>(_buttonUpdate, nameof(_buttonUpdate), gameObject);
        Tools.CheckNull<Button>(_buttonLater, nameof(_buttonLater), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_textLater, nameof(_textLater), gameObject);
        Tools.CheckNull<TextMeshProUGUI>(_description, nameof(_description), gameObject);

        if (String.IsNullOrEmpty(_DevEnvironmentID))
            Tools.LogError("UpdatesPopup: String _DevEnvironmentID is NULL or Empty");
    }

    public bool IsCheckForUpdates()
    {
        return _gameSettingsSO.IsCheckForUpdates == false;
    }

    private void Update()
    {
        if (_isStart == false)
            return;
        
        if (_isButtonLaterInteractable)
            return;
        
        _closeWaitDuration -= Time.deltaTime;
        if (_closeWaitDuration <= 0)
        {
            _textLater.text = "Later";
            _buttonLater.interactable = true;
            _isButtonLaterInteractable = true;
            _isStart = false;
        }
        else
        {
            _textLater.text = "Later [" + Mathf.RoundToInt(_closeWaitDuration).ToString() + "]";
        }
    }

    // Canvas > UILayerPopup > CheckingForUpdates > Popup_Update > Button_Update
    public void UpdateNow()
    {
        Application.OpenURL(_url);
        gameObject.SetActive(false);
    }

    // Canvas > UILayerPopup > CheckingForUpdates > Popup_Update > Button_Later
    public void Close()
    {
        if (_isButtonLaterInteractable)
        {
            gameObject.SetActive(false);
        }
    }

    private void Hide()
    {
        gameObject.SetActive(true);
        _mainTransformScale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }

    private void Show()
    {
        gameObject.transform.localScale = _mainTransformScale;
    }

    public struct userAttributes { }

    public struct appAttributes
    {
        public float VersionPC;
        public string VersionPCUrl;
        public string VersionPCDescription;
    }

    public async void Check(float currentVersion)
    {
        _gameSettingsSO.IsCheckForUpdates = true;
        await StartCheck(currentVersion);
    }

    public async Task StartCheck(float currentVersion)
    {
        // Debug.Log("UpdatesPopup: Check() currentVersion = " + currentVersion.ToString());
        if (Utilities.CheckForInternetConnection())
        {
            await InitializeRemoteConfigAsync();
        }

        Hide();
        _currentVersion = currentVersion;
        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
        
        if (_isDevelopment)
        {
            // Debug.Log("UpdatesPopup: EnvironmentID is development.");
            RemoteConfigService.Instance.SetEnvironmentID(_DevEnvironmentID);
        }
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
    }

    private async Task InitializeRemoteConfigAsync()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    private void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        RemoteConfigService.Instance.FetchCompleted -= ApplyRemoteSettings;
        switch (configResponse.requestOrigin)
        {
            case ConfigOrigin.Default:
                // Debug.Log("ConfigOrigin.Default: Default values will be returned.");
                gameObject.SetActive(false);
                break;
            case ConfigOrigin.Cached:
                // Debug.Log("ConfigOrigin.Cached: Cached values loaded.");
                gameObject.SetActive(false);
                break;
            case ConfigOrigin.Remote:
                // Debug.Log("ConfigOrigin.Remote: Remote Values changed.");
                // Debug.Log("RemoteConfigService.Instance.appConfig fetched: " + RemoteConfigService.Instance.appConfig.config.ToString());

                #if UNITY_STANDALONE_WIN
                    _serverVersion = RemoteConfigService.Instance.appConfig.GetFloat("VersionPC");
                    _url = RemoteConfigService.Instance.appConfig.GetString("VersionPCUrl");
                    _description.text = RemoteConfigService.Instance.appConfig.GetString("VersionPCDescription");
                #endif

                #if UNITY_ANDROID
                    _serverVersion = RemoteConfigService.Instance.appConfig.GetFloat("VersionAndroid");
                    _url = RemoteConfigService.Instance.appConfig.GetString("VersionAndroidUrl");
                    _description.text = RemoteConfigService.Instance.appConfig.GetString("VersionAndroidDescription");
                #endif

                #if UNITY_IOS
                    _serverVersion = RemoteConfigService.Instance.appConfig.GetFloat("VersioniOS");
                    _url = RemoteConfigService.Instance.appConfig.GetString("VersioniOSUrl");
                    _description.text = RemoteConfigService.Instance.appConfig.GetString("VersioniOSDescription");
                #endif

                if (_serverVersion > _currentVersion)
                {
                    Show();
                    _buttonUpdate.interactable = true;
                    _isStart = true;
                }
                else
                {
                    gameObject.SetActive(false);
                }
                break;
        }
    }

}
