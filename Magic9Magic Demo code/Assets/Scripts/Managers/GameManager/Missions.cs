public class Missions
{
    public enum Type
    {
        TimerType,
        ExperienceType,
        KillTargetType,
        KillAllType
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
        PlayerDied,
        TimeOver,
        AlarmTriggered
    }
}
