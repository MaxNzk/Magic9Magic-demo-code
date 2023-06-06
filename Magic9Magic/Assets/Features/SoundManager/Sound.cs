using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _baseVolumeLevel;
    private float _basePitchLevel;
    private Transform _transform;
    private Vector3 _centerPosition;
    [SerializeField] private List<AudioClip> _clipList = new List<AudioClip>();
    private AudioClip _lastClip;
    [SerializeField] private bool _isInterrupted;
    [field: SerializeField] public bool IsParallelPlayback { get; private set; }
    [field: SerializeField] public int ParallelSoundAmount { get; private set; }

    [Header("------------------------------------------")]
    [Header("is looped:")]
    [SerializeField] private bool _isAutoplay;
    [Header("only for 3d Audio:")]
    [SerializeField] private bool _isRandomPosition;
    [Header("the center is at the current position of the gameObject")]
    [SerializeField] private float _radiusRandomPosition = 100f;
    [Header("waiting between plays in sec:")]
    [SerializeField] private float _minWaiting = 2.2f;
    [SerializeField] private float _maxWaiting = 3.2f;

    [Header("------------------------------------------")]
    [SerializeField] private bool _isRandomVolume;
    [SerializeField] [Range(0, 0.5f)] private float _volumeOffset = 0.1f;
    [SerializeField] private bool _isRandomPitch;
    [SerializeField] [Range(0, 0.5f)] private float _pitchOffset = 0.1f;
    [SerializeField] private bool _isRandomPlayDelay;
    [SerializeField] [Range(0, 0.5f)] private float _minPlayDelay = 0.1f;
    [SerializeField] [Range(0, 2.0f)] private float _maxPlayDelay = 1.5f;

    [Header("------------------------------------------")]
    [SerializeField] private bool _isRandomProbability;
    [SerializeField] [Range(0, 100)] private int _percentProbability = 50;
    [Header("(Not) Successful no more than X times in a row")]
    [SerializeField] [Range(0, 5)] private int _successfulInARow = 2;
    [SerializeField] [Range(0, 10)] private int _notSuccessfulInARow = 3;
    private int _successfulInARowCount;
    private int _notSuccessfulInARowCount;

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (_isInterrupted && IsParallelPlayback)
        {
            _isInterrupted = false;
            IsParallelPlayback = false;
        }
        if (_radiusRandomPosition <= 0)
        {
            _radiusRandomPosition = 100f;
        }
        if (_minWaiting <= 0)
        {
            _minWaiting = 2.2f;
        }
        if (_maxWaiting <= 0)
        {
            _maxWaiting = 3.2f;
        }
    }
    #endif

    private void Awake()
    {
        FindAndTestComponents();
        SetBaseParameters();
        StartAutoplay();
    }

    private void FindAndTestComponents()
    {
        _transform = GetComponent<Transform>();
        _audioSource = Tools.GetComponentWithAssertion<AudioSource>(gameObject);

        if (_audioSource.outputAudioMixerGroup == null)
            Tools.LogError(gameObject.name + ": AudioSource.Output = NULL");
        if (_clipList.Count == 0)
            Tools.LogError(gameObject.name + ": _clipList is Empty!");
    }

    private void SetBaseParameters()
    {
        _centerPosition = _transform.position;
        _baseVolumeLevel = _audioSource.volume;
        _basePitchLevel = _audioSource.pitch;
        _lastClip = _clipList[0];
    }

    private void StartAutoplay()
    {
        if (_isAutoplay)
        {
            StartCoroutine (WaitAndPlayAutoplay());
        }
    }

    private IEnumerator WaitAndPlayAutoplay()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minWaiting, _maxWaiting));
            if (_isRandomPosition)
            {
                float xPos = Random.Range(_centerPosition.x, _centerPosition.x + _radiusRandomPosition);
                float zPos = Random.Range(_centerPosition.z, _centerPosition.z + _radiusRandomPosition);
                _transform.position = new Vector3(xPos, _transform.position.y, zPos);
            }
            Play2dSound();            
        }
    }

    public void Play3dSound(Vector3 position)
    {
        _transform.position = position;
        Play2dSound();
    }

    public void Play2dSound()
    {
        SetClip();
        SetVolume();
        SetPitch();
        if (_isInterrupted)
        {
            if (CheckProbability())
            {
                if (_isRandomPlayDelay)
                {
                    StartCoroutine(WaitAndPlay());
                }
                else
                {
                    _audioSource.Play();
                }
            }
        }
        else
        {
            if (_audioSource.isPlaying == false && CheckProbability())
            {
                if (_isRandomPlayDelay)
                {
                    StartCoroutine(WaitAndPlay());
                }
                else
                {
                    _audioSource.Play();
                }
            }
        }
    }

    private IEnumerator WaitAndPlay()
    {
        float randomDelayTime = Random.Range(_minPlayDelay, _maxPlayDelay);
        yield return new WaitForSeconds(randomDelayTime);
        _audioSource.Play();
    }

    public void Pause() => _audioSource.Pause();
    public void Stop() => _audioSource.Stop();
    public bool IsPlaying() => _audioSource.isPlaying;

    #if UNITY_EDITOR
    public void PlayInEditor()
    {
        _audioSource = GetComponent<AudioSource>();
        float tmpVolume = _audioSource.volume;
        float tmpPitch = _audioSource.pitch;
        _baseVolumeLevel = _audioSource.volume;
        _basePitchLevel = _audioSource.pitch;
        Play2dSound();
        _audioSource.volume = tmpVolume;
        _audioSource.pitch = tmpPitch;
    }
    #endif

    private void SetClip()
    {
        if (_clipList.Count == 1)
        {
            _audioSource.clip = _clipList[0];
        }
        else
        {
            int attempts = 3;
            AudioClip newClip = _clipList[Random.Range(0, _clipList.Count)];
            while (newClip == _lastClip && attempts > 0) 
            {
                newClip = _clipList[Random.Range(0, _clipList.Count)];
                attempts--;
            }
            _lastClip = newClip;
            _audioSource.clip = newClip;
        }
    }

    private void SetVolume()
    {
        if (_isRandomVolume && _volumeOffset != 0)
        {
            float minValue = Mathf.Clamp(_baseVolumeLevel - _volumeOffset, 0, 1.0f);
            float maxValue = Mathf.Clamp(_baseVolumeLevel + _volumeOffset, 0, 1.0f);
            _audioSource.volume = Random.Range(minValue, maxValue);
        }
    }

    private void SetPitch()
    {
        if (_isRandomPitch && _pitchOffset != 0)
        {
            float minValue = Mathf.Clamp(_basePitchLevel - _pitchOffset, -3.0f, 3.0f);
            float maxValue = Mathf.Clamp(_basePitchLevel + _pitchOffset, -3.0f, 3.0f);
            _audioSource.pitch = Random.Range(minValue, maxValue);
        }
    }

    private bool CheckProbability()
    {
        if (_isRandomProbability == false)
            return true;
        
        int randomValue = Random.Range(0, 100 / _percentProbability);
        if (randomValue == 0)
        {
            _successfulInARowCount++;
            if (_successfulInARowCount <= _successfulInARow)
            {
                _notSuccessfulInARowCount = 0;
                return true;
            }
            else
            {
                _successfulInARowCount = 0;
                _notSuccessfulInARowCount = 1;
                return false;
            }
        }
        else
        {
            _notSuccessfulInARowCount++;
            if (_notSuccessfulInARowCount > _notSuccessfulInARow)
            {
                _successfulInARowCount = 1;
                _notSuccessfulInARowCount = 0;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
