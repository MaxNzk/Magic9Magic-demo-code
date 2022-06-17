using UnityEngine;
using UnityEngine.Audio;

public class SoundManagerSettings : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettingsSO;
    [SerializeField] private AudioMixer _mainAudioMixer;
    [SerializeField] private AudioSource _audioSourceClick;
    
    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckSingleInstance<SoundManagerSettings>(gameObject);

        if (_gameSettingsSO == null)
            Tools.LogError("GameSettings _gameSettings = NULL");
        if (_mainAudioMixer == null)
            Tools.LogError("AudioMixer _mainAudioMixer = NULL");

        if (_audioSourceClick == null)
            Tools.LogError("AudioSource _audioSourceClick = NULL");
        if (_audioSourceClick.clip == null)
            Tools.LogError("_audioSourceClick.clip == NULL");
    }

    public void ApplySoundSettings()
    {
        _mainAudioMixer.SetFloat("MusicVolume", NormolizeVolume(_gameSettingsSO.MusicVolume));
        _mainAudioMixer.SetFloat("EffectsVolume", NormolizeVolume(_gameSettingsSO.EffectsVolume));
        _mainAudioMixer.SetFloat("AmbientVolume", NormolizeVolume(_gameSettingsSO.AmbientVolume));
        if (_gameSettingsSO.IsSound)
        {
            _mainAudioMixer.SetFloat("MasterVolume", 0);
        }
        else
        {
            _mainAudioMixer.SetFloat("MasterVolume", -80.0f);
        }
    }

    private float NormolizeVolume(float volume)
    {
        return volume < 0.3f ? Mathf.Lerp(-80, 150, volume) : Mathf.Lerp(-20, 0, volume);
    }

    public void PlaySoundClick() // For Canvas elements
    {
        _audioSourceClick.Play();
    }
}
