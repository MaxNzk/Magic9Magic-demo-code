using System.Collections;
using UnityEngine;

public class ToCampPortal : MonoBehaviour
{   
    [SerializeField] private string _campSceneName = "CampScene";
    [SerializeField] private float _waitForTeleportationInSec = 2.5f;
    [SerializeField] private SoundManager.SoundNamesUI soundNameAppearing;
    [SerializeField] private SoundManager.SoundNamesUI soundNameSending;
    [SerializeField] private bool _isGizmos = true;

    private IPassedThroughPortal _gameManager;
    private IAnalytics _analytics;
    private ISoundManager _soundManager;
    private bool _isInitialized;

    public void Initialize(IPassedThroughPortal gameManager, IAnalytics analytics, ISoundManager soundManager)
    {
        if (_isInitialized)
            return;

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
        _gameManager.PassedThroughPortal(_campSceneName, "", -1);
    }

    private void OnDrawGizmos()
    {
        if (_isGizmos)
        {
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawSphere(transform.position, 2.5f);
        }
    }

    public void Show(Vector3 position)
    {
        gameObject.transform.position = position;
        _soundManager.Play(soundNameAppearing.ToString());
        gameObject.SetActive(true);
    }

}
