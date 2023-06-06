using UnityEngine;

public class FinishMissionSO : ScriptableObject
{

    public virtual bool Finish(Missions.Status missionStatus)
    {
        if (missionStatus == Missions.Status.BackToCamp)
            return false;
        
        if (missionStatus == Missions.Status.PlayerDied)
            return false;
        
        if (missionStatus == Missions.Status.AlarmTriggered)
            return false;

        return true;
    }

}
