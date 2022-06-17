using System.Collections;
using UnityEngine;

public class ToCampPortal : MonoBehaviour
{   
    [SerializeField] private string _campSceneName = "CampScene";
    [SerializeField] private float _waitForTeleportationInSec = 2.5f;
    [SerializeField] private bool _isAppearsNearPlayer;
    [SerializeField] private SoundManager.SoundNames soundNameAppearing;
    [SerializeField] private SoundManager.SoundNames soundNameSending;
    [SerializeField] private bool _isGizmos = true;
    private IPassedThroughPortal _gameManager;
    private GameAnalytics _analytics;
    private SoundManager _soundManager;
    private bool _isInitialized;

    public void Initialize(IPassedThroughPortal gameManager, GameAnalytics analytics, SoundManager soundManager)
    {
        if (_isInitialized) return;

        _gameManager = gameManager;
        _analytics = analytics;
        _soundManager = soundManager;
        FindAndTestComponents();
        _isInitialized = true;
    }

    private void FindAndTestComponents()
    {
        if (string.IsNullOrEmpty(_campSceneName))
            Tools.LogError("_campSceneName is Null or Empty");
        if (_waitForTeleportationInSec <= 0)
            Tools.LogError("_waitForTeleportationInSec <= 0");
        if (_gameManager == null)
            Tools.LogError("IPassedThroughPortal _gameManager = NULL");
        if (_analytics == null)
            Tools.LogError("GameAnalytics _analytics = NULL");
        if (_soundManager == null)
            Tools.LogError("SoundManager _soundManager = NULL");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IPortalable>(out IPortalable portalable))
        {
            portalable.Disappear();
            StartCoroutine(StartTeleportation());
        }
    }

    private IEnumerator StartTeleportation()
    {
        yield return new WaitForSeconds(_waitForTeleportationInSec);
        _soundManager.Play(soundNameSending.ToString());
        _analytics.PassedThroughPortal(_campSceneName, "");
        _gameManager.PassedThroughPortal(_campSceneName, "");
    }

    private void OnDrawGizmos()
    {
        if (_isGizmos)
        {
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawSphere(transform.position, 2.5f);
        }
    }

    public void Show()
    {
        _soundManager.Play(soundNameAppearing.ToString());
        if (_isAppearsNearPlayer)
        {
            // Vector3 from GameManagerWorld1 < playerMovement
        }
        gameObject.SetActive(true);
    }

    public void Show(Vector3 position)
    {
        gameObject.transform.position = position;
        Show();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
