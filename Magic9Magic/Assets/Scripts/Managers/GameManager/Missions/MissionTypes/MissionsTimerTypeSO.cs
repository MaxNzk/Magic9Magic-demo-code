using UnityEngine;

[CreateAssetMenu(fileName = "MissionsTimerTypeSO", menuName = "Scriptable Objects/MissionTypes/MissionsTimerType")]
public class MissionsTimerTypeSO : FinishMissionSO
{

    public override bool Finish(Missions.Status missionStatus)
    {
        if (base.Finish(missionStatus) == false)
            return false;

        if (missionStatus == Missions.Status.TimeOver)
            return true;

        return true;
    }

}
