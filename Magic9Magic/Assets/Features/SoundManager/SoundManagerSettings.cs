using UnityEngine;
using UnityEngine.Audio;

public class SoundManagerSettings : MonoBehaviour
{
    [SerializeField] private GameSettingsSO _gameSettingsSO;
    [SerializeField] private AudioMixer _mainAudioMixer;
    [SerializeField] private AudioSource _audioSourceClick;
    
    private void Awake()
    {
        FindAndTestComponents();
    }

    private void FindAndTestComponents()
    {
        Tools.CheckSingleInstance<SoundManagerSettings>(gameObject);

        Tools.CheckNull<GameSettingsSO>(_gameSettingsSO, nameof(_gameSettingsSO), gameObject);
        Tools.CheckNull<AudioMixer>(_mainAudioMixer, nameof(_mainAudioMixer), gameObject);

        _audioSourceClick = Tools.GetComponentWithAssertion<AudioSource>(gameObject);
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
