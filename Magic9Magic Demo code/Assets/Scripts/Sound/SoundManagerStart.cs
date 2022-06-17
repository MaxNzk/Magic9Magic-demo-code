using UnityEngine;
using UnityEngine.Audio;

public class SoundManagerStart : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettingsSO;
    [SerializeField] private AudioMixer _mainAudioMixer;
    private AudioSource _audioSource;
    
    public void Initialize()
    {
        FindAndTestComponents();    
        ApplySoundSettings();
        PlayStartSound();
    }

    private void FindAndTestComponents()
    {
        _audioSource = GetComponent<AudioSource>();
        Tools.CheckSingleInstance<SoundManagerStart>(gameObject);
            
        if (_gameSettingsSO == null)
            Tools.LogError("GameSettings _gameSettings = NULL");
        if (_mainAudioMixer == null)
            Tools.LogError("AudioMixer _mainAudioMixer = NULL");
        if (_audioSource == null)
            Tools.LogError("AudioSource _audioSource = NULL");
        if (_audioSource.clip == null)
            Tools.LogError("_audioSource.clip == NULL");
    }

    private void PlayStartSound()
    {
        _audioSource.Play();
    }

    private void ApplySoundSettings()
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

}
