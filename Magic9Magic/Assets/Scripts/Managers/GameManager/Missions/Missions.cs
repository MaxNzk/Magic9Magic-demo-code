using System;

public class Missions
{
    public static string ParamsToString(Params missionParams)
    {
        string result = "";
        if ((missionParams & Params.CountdownTimerMode) != 0) result += "Timer";
        if ((missionParams & Params.ManaLimitMode) != 0) result += "Mana";
        if ((missionParams & Params.StealthMode) != 0) result += "Stealth";
        if (String.IsNullOrEmpty(result))
            result = "None";
        else
            result += "Mode";
        return result;
    }

    public enum Type
    {
        TimerType = 5,
        ExperienceType = 10,
        KillTargetType = 15,
        KillAllType = 20,
        ReachTarget = 25
    }

    [System.Flags]
    public enum Params
    {
        CountdownTimerMode = 1,
        ManaLimitMode = 2,
        StealthMode = 4
    }

    public enum Status
    {
        PlayerDied = 5,
        TimeOver = 10,
        AlarmTriggered = 15,
        ReachedTarget = 20,
        BackToCamp = 25
    }
}
