using UnityEngine;
using UnityEngine.Playables;

public class CSP_CampFirst : CutscenePlayer
{
    [Space(10)]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PlayerMovement _playerMovementScript;

    [Space(10)]
    [SerializeField] private Transform _startPortalPosition;
    [SerializeField] private Transform _stopPortalPosition;

    protected override void Awake()
    {
        base.Awake();

        Tools.CheckNull<Transform>(_playerTransform, nameof(_playerTransform), gameObject);
        Tools.CheckNull<PlayerMovement>(_playerMovementScript, nameof(_playerMovementScript), gameObject);
        Tools.CheckNull<Transform>(_startPortalPosition, nameof(_startPortalPosition), gameObject);
        Tools.CheckNull<Transform>(_stopPortalPosition, nameof(_stopPortalPosition), gameObject);
    }

    protected override void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        _playerMovementScript.SetSpeedParam(isAlwaysRunning: true);

        base.OnPlayableDirectorStopped(_PlayableDirector);
    }

    public override void Play()
    {
        _playerTransform.position = _startPortalPosition.position;
        _playerMovementScript.SetTarget(_stopPortalPosition);
        _playerMovementScript.SetSpeedParam(isAlwaysRunning: false);

        base.Play();
    }

    public override bool ToCutscene()
    {
        if (_cutsceneSettingsSO.IsCutsceneCampFirst)
        {
            return false;
        }
        else
        {
            _cutsceneSettingsSO.IsCutsceneCampFirst = true;
            return true;
        }
    }

}
