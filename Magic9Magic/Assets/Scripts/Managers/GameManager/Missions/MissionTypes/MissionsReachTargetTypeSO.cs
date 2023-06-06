using UnityEngine;

[CreateAssetMenu(fileName = "MissionsReachTargetTypeSO", menuName = "Scriptable Objects/MissionTypes/MissionsReachTargetsType")]
public class MissionsReachTargetTypeSO : FinishMissionSO
{

    public override bool Finish(Missions.Status missionStatus)
    {
        if (base.Finish(missionStatus) == false)
            return false;

        if (missionStatus == Missions.Status.TimeOver)
            return false;
        
        if (missionStatus == Missions.Status.ReachedTarget)
            return true;

        return true;
    }

}

