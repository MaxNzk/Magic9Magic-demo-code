using UnityEngine;

public class Portal : MonoBehaviour
{   
    [SerializeField] private WorldPortalColorSO _worldPortalColorSO; 
    [SerializeField] private WorldDataSO _worldDataSO;
    [SerializeField] private SoundManager.SoundNames soundNameAppearing;
    [SerializeField] private SoundManager.SoundNames soundNameSending;
    private IPassedThroughPortal _gameManager;
    private GameAnalytics _analytics;
    private SoundManager _soundManager;
    private bool _isInitialized;
    [SerializeField] private Renderer _renderer;

    public void Initialize(IPassedThroughPortal gameManager, GameAnalytics analytics, SoundManager soundManager)
    {
        if (_isInitialized) return;

        _gameManager = gameManager;
        _analytics = analytics;
        _soundManager = soundManager;
        FindAndTestComponents();
        _renderer.material.color = _worldPortalColorSO.PortalColor;
        _isInitialized = true;
    }

    private void FindAndTestComponents()
    {
        if (_worldPortalColorSO == null)
            Tools.LogError("WorldPortalColorSO _worldPortalColorSO = NULL");
        if (_worldDataSO == null)
            Tools.LogError("WorldDataSO _worldDataSO = NULL");
        if (string.IsNullOrEmpty(_worldPortalColorSO.SceneName))
            Tools.LogError("Setup portal world snene name!");
        if (_gameManager == null)
            Tools.LogError("IPassedThroughPortal _gameManager = NULL");
        if (_analytics == null)
            Tools.LogError("GameAnalytics _analytics = NULL");
        if (_soundManager == null)
            Tools.LogError("SoundManager _soundManager = NULL");
        if (_renderer == null)
            Tools.LogError("Renderer _rendorer = NULL");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IPortalable>(out IPortalable portalable))
        {
            _soundManager.Play(soundNameSending.ToString());
            _analytics.PassedThroughPortal(_worldPortalColorSO.SceneName, _worldDataSO.WorldSOName);
            _gameManager.PassedThroughPortal(_worldPortalColorSO.SceneName, _worldDataSO.WorldSOName);
            portalable.Disappear();
        }
    }

    public void Show()
    {
        _soundManager.Play(soundNameAppearing.ToString());
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
