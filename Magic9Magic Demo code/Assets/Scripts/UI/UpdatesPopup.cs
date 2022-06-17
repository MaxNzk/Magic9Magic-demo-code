using System;
using UnityEngine;
using Unity.RemoteConfig;
using UnityEngine.UI;
using TMPro;

public class UpdatesPopup : MonoBehaviour
{
    [Header("Button Later ---------------------------")]
    [SerializeField] private float _closeWaitDuration = 10.0f;
    [SerializeField] private Button _buttonUpdate;
    [SerializeField] private Button _buttonLater;
    [SerializeField] private TextMeshProUGUI _textLater;
    private bool _isButtonLaterInteractable;
    [Space(10)]
    [Header("Remote Config ---------------------------")]
    [SerializeField] private bool _isDevelopment;
    [SerializeField] private string _DevEnvironmentID;
    [SerializeField] private TextMeshProUGUI _description;
    private int _serverVersion;
    private int _currentVersion;
    private string _url;
    private Vector3 _mainTransformScale;
    private bool _isStart;

    private void Start()
    {
        FindAndTestComponents();
        _buttonLater.interactable = false;
        _buttonUpdate.interactable = false;
    }

    private void FindAndTestComponents()
    {
        if (_closeWaitDuration <= 0)
            Tools.LogError("_closeWaitDuration <= 0");
        if (_buttonUpdate == null)
            Tools.LogError("Button _buttonUpdate = NULL");
        if (_buttonLater == null)
            Tools.LogError("Button _buttonLater = NULL");
        if (_textLater == null)
            Tools.LogError("TextMeshProUGUI _textLater = NULL");
        if (String.IsNullOrEmpty(_DevEnvironmentID))
            Tools.LogError("String _DevEnvironmentID is NULL or Empty");
        if (_description == null)
            Tools.LogError("TextMeshProUGUI _description = NULL");
    }

    private void Update()
    {
        if (_isStart == false) return;

        if (_isButtonLaterInteractable == false)
        {
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
        
    }

    public void UpdateNow()
    {
        Application.OpenURL(_url);
        gameObject.SetActive(false);
    }

    public void Close()
    {
        if (_isButtonLaterInteractable)
            gameObject.SetActive(false);
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

    public struct userAttributes { } //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Can be private???

    public struct appAttributes
    {
        public int VersionPC;
        public string VersionPCUrl;
        public string VersionPCDescription;
    }

    public void Check(int currentVersion)
    {
        Hide();
        _currentVersion = currentVersion;
        ConfigManager.FetchCompleted += ApplyRemoteSettings;
        if (_isDevelopment)
            ConfigManager.SetEnvironmentID(_DevEnvironmentID);
        ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
    }

    private void ApplyRemoteSettings (ConfigResponse configResponse)
    {
        ConfigManager.FetchCompleted -= ApplyRemoteSettings;
        switch (configResponse.requestOrigin)
        {
            case ConfigOrigin.Default:
                gameObject.SetActive(false);
                break;
            case ConfigOrigin.Cached:
                gameObject.SetActive(false);
                break;
            case ConfigOrigin.Remote:
                _serverVersion = ConfigManager.appConfig.GetInt ("VersionPC");
                _url = ConfigManager.appConfig.GetString ("VersionPCUrl");
                _description.text = ConfigManager.appConfig.GetString ("VersionPCDescription");
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
