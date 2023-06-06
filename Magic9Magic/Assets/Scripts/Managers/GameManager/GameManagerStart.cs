using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerStart : MonoBehaviour
{
    [Header("----- Managers: ------------------------------")]
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private SoundManagerStart _soundManager;
    [SerializeField] private GameObject _analyticsGO;
    private IAnalytics _analytics;

    [Space(10)]
    [Header("----- ScriptableObjects: ---------------------")]
    [SerializeField] private GameSettingsSO _gameSettingsSO;
    [SerializeField] private AnalyticsStorage _analyticsStorageSO;
    private SceneFade _sceneFade;
    private bool _isFirstStartEver;

    private void Awake()
    {
        Tools.Log("--------- Start Scene -----------");
        FindAndTestComponents();
        LoadGraphicsQuality();
        Initialize();
        SetDefaultGameSettings();
        StartNewGameSession();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckSingleInstance<GameManagerStart>(gameObject);

        Tools.CheckNull<SaveManager>(_saveManager, nameof(_saveManager), gameObject);
        Tools.CheckNull<SoundManagerStart>(_soundManager, nameof(_soundManager), gameObject);

        _analytics = _analyticsGO.GetComponent<IAnalytics>();
        Tools.CheckNull<IAnalytics>(_analytics, nameof(_analytics), gameObject);
        
        Tools.CheckNull<GameSettingsSO>(_gameSettingsSO, nameof(_gameSettingsSO), gameObject);
        Tools.CheckNull<AnalyticsStorage>(_analyticsStorageSO, nameof(_analyticsStorageSO), gameObject);

        _sceneFade = FindObjectOfType<SceneFade>();
        Tools.CheckNull<SceneFade>(_sceneFade, nameof(_sceneFade), gameObject);
    }

    private void LoadGraphicsQuality()
    {
        QualitySettings.SetQualityLevel (_gameSettingsSO.GraphicsLevel, true);
    }

    private void SetDefaultGameSettings()
    {
        _gameSettingsSO.CurrentVersion = float.Parse(Application.version, CultureInfo.InvariantCulture.NumberFormat);
        _gameSettingsSO.IsCheckForUpdates = false;
    }

    private void StartNewGameSession()
    {
        _analytics.StartNewGameSession();
    }

    private void Initialize()
    {
        _saveManager.Initialize();
        _soundManager.Initialize();
        _analytics.Initialize(_analyticsStorageSO);

        _isFirstStartEver = _analyticsStorageSO.FirstPathStep == 0 ? true : false;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds (3.0f);
        _sceneFade.FadeIn();
        ChooseNextScene();
    }

    private void ChooseNextScene()
    {
        if (_isFirstStartEver)
        {
            _analyticsStorageSO.FirstPathStep++;
            StartCoroutine(LoadScene("SettingsScene"));         
        }
        else
        {
            StartCoroutine(LoadScene("CampScene"));
        }
    }

    private IEnumerator LoadScene(string sceneName)
    {
		yield return new WaitForSeconds (1.0f);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
		yield return async;
	}

}
