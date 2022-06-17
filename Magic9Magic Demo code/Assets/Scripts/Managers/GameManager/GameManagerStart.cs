using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerStart : MonoBehaviour
{
    [SerializeField] private bool _isReSaveAllSO;
    [Header("----- Managers: ------------------------------")]
    [SerializeField] private GameAnalytics _analytics;
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private SoundManagerStart _soundManager;
    [Space(10)]
    [Header("----- ScriptableObjects: ---------------------")]
    [SerializeField] private GameSettings _gameSettingsSO;
    [SerializeField] private AnalyticsStorage _analyticsStorageSO;
    private SceneFade _sceneFade;
    private bool _isFirstStartEver;

    private void Awake()
    {
        FindAndTestComponents();
        if (_isReSaveAllSO)
            _isFirstStartEver = true;
        _saveManager.Initialize(_isFirstStartEver);
        _isFirstStartEver = _analyticsStorageSO.FirstPathStep == 0 ? true : false;
        _analytics.Initialize();
        _analytics.TakeNextFirstStep(0);
        _soundManager.Initialize();
        LoadGraphicsQuality();
        SetDefaultGameSettings();
    }

    private void FindAndTestComponents()
    {
        _sceneFade = FindObjectOfType<SceneFade>();

        Tools.CheckSingleInstance<GameManagerStart>(gameObject);
        if (_gameSettingsSO == null)
            Tools.LogError("GameSettings _gameSettingsSO = NULL");
        if (_soundManager == null)
            Tools.LogError("SoundManagerStart _soundManager = NULL");
        if (_saveManager == null)
            Tools.LogError("SaveManager _saveManager = NULL");
        if (_analytics == null)
            Tools.LogError("GameAnalytics _analytics = NULL");
        if (_sceneFade == null)
            Tools.LogError("SceneFade _sceneFade = NULL");
    }

    private void LoadGraphicsQuality()
    {
        QualitySettings.SetQualityLevel (_gameSettingsSO.GraphicsLevel, true);
    }

    private void SetDefaultGameSettings()
    {
        _gameSettingsSO.IsCheckForUpdates = false;
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
