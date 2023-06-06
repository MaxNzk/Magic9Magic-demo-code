using UnityEngine;
using UnityEngine.Playables;

public abstract class CutscenePlayer : MonoBehaviour
{
    public bool IsCutsceneStopped { get; private set; }
    [SerializeField] protected PlayableAsset _cutscene;
    protected PlayableDirector _PlayableDirector;

    protected CutsceneSettingsSO _cutsceneSettingsSO;
    protected InputManager _inputManager;

    public void Initialize(CutsceneSettingsSO cutsceneSettingsSO, InputManager inputManager)
    {
        _cutsceneSettingsSO = cutsceneSettingsSO;
        _inputManager = inputManager;
    }
    
    protected virtual void Awake()
    {
        _PlayableDirector = Tools.GetComponentWithAssertion<PlayableDirector>(gameObject);
        _PlayableDirector.playableAsset = _cutscene;
        IsCutsceneStopped = true;
    }

    protected virtual void OnEnable()
    {
        _PlayableDirector.stopped += OnPlayableDirectorStopped;
    }

    protected virtual void OnDisable()
    {
        _PlayableDirector.stopped -= OnPlayableDirectorStopped;
    }

    protected virtual void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        _inputManager.Activate();
        IsCutsceneStopped = true;
    }

    public virtual void Play()
    {
        IsCutsceneStopped = false;
        _inputManager.Deactivate();
        _PlayableDirector.Play();
    }

    public abstract bool ToCutscene();

}
