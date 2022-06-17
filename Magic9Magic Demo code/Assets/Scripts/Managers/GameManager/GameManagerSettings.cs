using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManagerSettings : MonoBehaviour
{
    [Header("----- Managers: ------------------------------")]
    [SerializeField] private SoundManagerSettings _soundManager;
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private GameAnalytics _analytics;
    [Space(10)]
    [Header("----- ScriptableObjects: ---------------------")]
    [SerializeField] private GameSettings _gameSettingsSO;
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
    private MyCVDFilter _cdvFilter;
    private TextMeshProUGUI _colorBMValueText;
    private SceneFade _sceneFade;

    private void Awake()
    {
        Tools.Log("--------- Settings -----------");
        FindAndTestComponents();
        _analytics.Initialize();
        _analytics.TakeNextFirstStep(1);
        _saveManager.Initialize(false);
        LoadSettings();
    }

    private void FindAndTestComponents()
    {
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
        _cdvFilter = FindObjectOfType<MyCVDFilter>();
        _colorBMValueText = GameObject.Find("ColorBMValueText").GetComponent<TextMeshProUGUI>();
        _sceneFade = FindObjectOfType<SceneFade>();
        _sceneFade.gameObject.SetActive(false);
        
        Tools.CheckSingleInstance<GameManagerSettings>(gameObject);

        if (_saveManager == null)
            Tools.LogError("SaveManager _saveManager = NULL");
        if (_analytics == null)
            Tools.LogError("GameAnalytics _analytics = NULL");
        if (_soundManager == null)
            Tools.LogError("SoundManagerSettings _soundManager = NULL");

        if (_gameSettingsSO == null)
            Tools.LogError("GameSettings _gameSettingsSO = NULL");

        if (_EnBtn == null)
            Tools.LogError("Button _EnBtn = NULL");
        if (_UaBtn == null)
            Tools.LogError("Button _UaBtn = NULL");
        if (_RuBtn == null)
            Tools.LogError("Button _RuBtn = NULL");        
        
        if (_lowBtn == null)
            Tools.LogError("Button _lowBtn = NULL");
        if (_middleBtn == null)
            Tools.LogError("Button _middleBtn = NULL");
        if (_highBtn == null)
            Tools.LogError("Button _highBtn = NULL");

        if (_soundToggle == null)
            Tools.LogError("Toggle _soundToggle = NULL");
        if (_musicSlider == null)
            Tools.LogError("Slider _musicSlider = NULL");
        if (_effectsSlider == null)
            Tools.LogError("Slider _effectsSlider = NULL");
        if (_ambientSlider == null)
            Tools.LogError("Slider _ambientSlider = NULL");

        if (_cdvFilter == null)
            Tools.LogError("MyCVDFilter _cdvFilter = NULL");
        if (_colorBMValueText == null)
            Tools.LogError("ColorBMValueText _colorBMValueText = NULL");

        if (_sceneFade == null)
            Tools.LogError("SceneFade _sceneFade = NULL");
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
            var colors = _UaBtn.colors;
            colors.normalColor = new Color32(225, 225, 1, 255);
            _UaBtn.colors = colors;
        }
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
        QualitySettings.SetQualityLevel (_gameSettingsSO.GraphicsLevel, true);
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
