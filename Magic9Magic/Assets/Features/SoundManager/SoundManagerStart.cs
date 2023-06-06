using UnityEngine;
using UnityEngine.Audio;

public class SoundManagerStart : MonoBehaviour
{
    [SerializeField] private GameSettingsSO _gameSettingsSO;
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
        Tools.CheckSingleInstance<SoundManagerStart>(gameObject);

        Tools.CheckNull<GameSettingsSO>(_gameSettingsSO, nameof(_gameSettingsSO), gameObject);
        Tools.CheckNull<AudioMixer>(_mainAudioMixer, nameof(_mainAudioMixer), gameObject);

        _audioSource = Tools.GetComponentWithAssertion<AudioSource>(gameObject);
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
