using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManagerSettings : MonoBehaviour
{
    [SerializeField] private SceneFade _sceneFade;

    [Header("----- Managers: ------------------------------")]
    [SerializeField] private SoundManagerSettings _soundManager;
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private CVDFilter _cdvFilter;
    [SerializeField] private GameObject _analyticsGO;
    private IAnalytics _analytics;

    [Space(10)]
    [Header("----- ScriptableObjects: ---------------------")]
    [SerializeField] private GameSettingsSO _gameSettingsSO;
    [SerializeField] private AnalyticsStorage _analyticsStorageSO;

    private TextMeshProUGUI _colorBMValueText;
    private Button _EnBtn;
    private Button _UaBtn;
    private Button _RuBtn;
    private Button _lowBtn;
    private Button _middleBtn;
    private Button _highBtn;
    private Toggle _soundToggle;
    private Slider _musicSlider;
    private Slider _effectsSlider;
    private Slider _ambientSlider;

    private void Awake()
    {
        Tools.Log("--------- Settings Scene -----------");
        FindAndTestComponents();
        Initialize();
        LoadSettings();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckSingleInstance<GameManagerSettings>(gameObject);

        Tools.CheckNull<SceneFade>(_sceneFade, nameof(_sceneFade), gameObject);
        Tools.CheckNull<SoundManagerSettings>(_soundManager, nameof(_soundManager), gameObject);
        Tools.CheckNull<SaveManager>(_saveManager, nameof(_saveManager), gameObject);

        _analytics = _analyticsGO.GetComponent<IAnalytics>();
        if (_analytics == null)
        {
            _analytics = FindObjectOfType<SingletonAnalytics>();
            Tools.CheckNull<IAnalytics>(_analytics, nameof(_analytics), gameObject);
        }

        Tools.CheckNull<GameSettingsSO>(_gameSettingsSO, nameof(_gameSettingsSO), gameObject);
        Tools.CheckNull<AnalyticsStorage>(_analyticsStorageSO, nameof(_analyticsStorageSO), gameObject);

        Tools.CheckNull<CVDFilter>(_cdvFilter, nameof(_cdvFilter), gameObject);

        _colorBMValueText = GameObject.Find("ColorBMValueText").GetComponent<TextMeshProUGUI>();
        Tools.CheckNull<TextMeshProUGUI>(_colorBMValueText, nameof(_colorBMValueText), gameObject);

        _EnBtn = GameObject.Find("EnBtn").GetComponent<Button>();
        _UaBtn = GameObject.Find("UaBtn").GetComponent<Button>();
        _RuBtn = GameObject.Find("RuBtn").GetComponent<Button>();
        _lowBtn = GameObject.Find("LowBtn").GetComponent<Button>();
        _middleBtn = GameObject.Find("MiddleBtn").GetComponent<Button>();
        _highBtn = GameObject.Find("HighBtn").GetComponent<Button>();
        _soundToggle = GameObject.Find("SoundToggle").GetComponent<Toggle>();
        _musicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        _effectsSlider = GameObject.Find("EffectsSlider").GetComponent<Slider>();
        _ambientSlider = GameObject.Find("AmbientSlider").GetComponent<Slider>();

        Tools.CheckNull<Button>(_EnBtn, nameof(_EnBtn), gameObject);
        Tools.CheckNull<Button>(_UaBtn, nameof(_UaBtn), gameObject);
        Tools.CheckNull<Button>(_RuBtn, nameof(_RuBtn), gameObject);
        Tools.CheckNull<Button>(_lowBtn, nameof(_lowBtn), gameObject);
        Tools.CheckNull<Button>(_middleBtn, nameof(_middleBtn), gameObject);
        Tools.CheckNull<Button>(_highBtn, nameof(_highBtn), gameObject);
        Tools.CheckNull<Toggle>(_soundToggle, nameof(_soundToggle), gameObject);
        Tools.CheckNull<Slider>(_musicSlider, nameof(_musicSlider), gameObject);
        Tools.CheckNull<Slider>(_effectsSlider, nameof(_effectsSlider), gameObject);
        Tools.CheckNull<Slider>(_ambientSlider, nameof(_ambientSlider), gameObject);      
    }

    private void Initialize()
    {
        _analytics.Initialize(_analyticsStorageSO);
        _analytics.TakeNextFirstStep(1);
        _saveManager.Initialize();
    }

    private void LoadSettings()
    {
        GetLanguage();
        GetGraphicsQuality();
        GetSoundSettings();
        GetColorBlindMode();
    }

    private void GetLanguage()
    {
        Tools.Log("_gameSettingsSO.Language = " + _gameSettingsSO.Language);
        
        if (_gameSettingsSO.Language == "English")
        {
            var colors = _EnBtn.colors;
            colors.normalColor = new Color32(225, 225, 1, 255);
            _EnBtn.colors = colors;
        }
        if (_gameSettingsSO.Language == "Ukraine")
        {
            var colors = _UaBtn.colors;
            colors.normalColor = new Color32(225, 225, 1, 255);
            _UaBtn.colors = colors;
        }
        if (_gameSettingsSO.Language == "Russian")
        {
            var colors = _RuBtn.colors;
            colors.normalColor = new Color32(225, 225, 1, 255);
            _RuBtn.colors = colors;
        }

        StartCoroutine(SetLanguageWithDelay());
    }

    private IEnumerator SetLanguageWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        SetLanguage(_gameSettingsSO.Language);
    }

    private void GetGraphicsQuality()
    {
        if (_gameSettingsSO.GraphicsLevel == 0)
        {
            var colors = _lowBtn.colors;
            colors.normalColor = new Color32(225, 225, 1, 255);
            _lowBtn.colors = colors;
        }
        if (_gameSettingsSO.GraphicsLevel == 2)
        {
            var colors = _middleBtn.colors;
            colors.normalColor = new Color32(225, 225, 1, 255);
            _middleBtn.colors = colors;
        }
        if (_gameSettingsSO.GraphicsLevel == 5)
        {
            var colors = _highBtn.colors;
            colors.normalColor = new Color32(225, 225, 1, 255);
            _highBtn.colors = colors;
        }
    }

    private void GetSoundSettings()
    {
        _soundToggle.isOn = _gameSettingsSO.IsSound;
        _musicSlider.value = _gameSettingsSO.MusicVolume;
        _effectsSlider.value = _gameSettingsSO.EffectsVolume;
        _ambientSlider.value = _gameSettingsSO.AmbientVolume;
    }

    private void GetColorBlindMode()
    {
        _colorBMValueText.text = _gameSettingsSO.ColorBlindType;
        _cdvFilter.SetCurrentColorType(_gameSettingsSO.ColorBlindType);
    }

    public void ColorBlindModeNext()
    {
        _colorBMValueText.text = _cdvFilter.GetNextColorType();
        _gameSettingsSO.ColorBlindType = _colorBMValueText.text;
    }

    public void ColorBlindModePrev()
    {
        _colorBMValueText.text = _cdvFilter.GetPrevColorType();
        _gameSettingsSO.ColorBlindType = _colorBMValueText.text;
    }

    //--------------------------------------------------------------------------------------------------------------------------------

    public void SetLanguage(string name)  // Canvas > SettingsPanel > ContainerPanel > LangPanel > En/Ua/RuBtn
    {
        Tools.Log("SetLanguage = " + name);
        Lean.Localization.LeanLocalization.SetCurrentLanguageAll(name);
        _gameSettingsSO.Language = name;

        var colorsTmp = _EnBtn.colors;
        colorsTmp.normalColor = new Color32(225, 225, 255, 255);
        colorsTmp.selectedColor = new Color32(225, 225, 255, 255);
        _EnBtn.colors = colorsTmp;
        _UaBtn.colors = colorsTmp;
        _RuBtn.colors = colorsTmp;

        if (name == "English")
        {
            var colors = _EnBtn.colors;
            colors.normalColor = new Color32(225, 225, 1, 255);
            colors.selectedColor = new Color32(225, 225, 1, 255);
            _EnBtn.colors = colors;
        }
        if (name == "Ukraine")
        {
            var colors = _UaBtn.colors;
            colors.normalColor = new Color32(225, 225, 1, 255);
            colors.selectedColor = new Color32(225, 225, 1, 255);
            _UaBtn.colors = colors;
        }
        if (name == "Russian")
        {
            var colors = _RuBtn.colors;
            colors.normalColor = new Color32(225, 225, 1, 255);
            colors.selectedColor = new Color32(225, 225, 1, 255);
            _RuBtn.colors = colors;
        }
    }

    public void NextScene() // Canvas > SettingsPanel > ContainerPanel > PlayBtn
    {
        _analytics.ChangeColorBlindType(_colorBMValueText.text);
        _soundManager.PlaySoundClick();
        QualitySettings.SetQualityLevel(_gameSettingsSO.GraphicsLevel, true);
        _sceneFade.FastFadeIn();
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds (1.0f);
        AsyncOperation async = SceneManager.LoadSceneAsync("CampScene");
		yield return async;
    }

    public void SetGraphicsQuality(int index)  // Canvas > SettingsPanel > ContainerPanel > GraphicsPanel > low/middle/highBtn
    {
        _gameSettingsSO.GraphicsLevel = index;

        var colorsTmp = _lowBtn.colors;
        colorsTmp.normalColor = new Color32(225, 225, 255, 255);
        colorsTmp.selectedColor = new Color32(225, 225, 255, 255);
        _lowBtn.colors = colorsTmp;
        _middleBtn.colors = colorsTmp;
        _highBtn.colors = colorsTmp;

        if (_gameSettingsSO.GraphicsLevel == 0)
        {
            var colors = _lowBtn.colors;
            colors.normalColor = new Color32(225, 225, 1, 255);
            colors.selectedColor = new Color32(225, 225, 1, 255);
            _lowBtn.colors = colors;
        }
        if (_gameSettingsSO.GraphicsLevel == 2)
        {
            var colors = _middleBtn.colors;
            colors.normalColor = new Color32(225, 225, 1, 255);
            colors.selectedColor = new Color32(225, 225, 1, 255);
            _middleBtn.colors = colors;
        }
        if (_gameSettingsSO.GraphicsLevel == 5)
        {
            var colors = _highBtn.colors;
            colors.normalColor = new Color32(225, 225, 1, 255);
            colors.selectedColor = new Color32(225, 225, 1, 255);
            _highBtn.colors = colors;
        }
    }

    public void SetSoundSettings(bool isOn)
    {
        _gameSettingsSO.IsSound = isOn;
        _soundManager.ApplySoundSettings();
    }

    public void SetMusicVolume(float value)
    {
        _gameSettingsSO.MusicVolume = value;
        _soundManager.ApplySoundSettings();
    }

    public void SetEffectsVolume(float value)
    {
        _gameSettingsSO.EffectsVolume = value;
        _soundManager.ApplySoundSettings();
    }

    public void SetAmbientVolume(float value)
    {
        _gameSettingsSO.AmbientVolume = value;
        _soundManager.ApplySoundSettings();
    }

}
