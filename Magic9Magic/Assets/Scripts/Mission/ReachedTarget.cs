using UnityEngine;

public class ReachedTarget : MonoBehaviour
{
    private IFinishMission _gameManagerIFinishMission;
    private Missions.Status _missionStatus = Missions.Status.ReachedTarget;

    public void Initialize(IFinishMission gameManagerIFinishMission)
    {
        _gameManagerIFinishMission = gameManagerIFinishMission;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IPortalable>(out IPortalable portalable))
        {
            portalable.SetDestinationAtPoint(transform.position);
            _gameManagerIFinishMission.FinishMission(_missionStatus);
        }
    }

}
