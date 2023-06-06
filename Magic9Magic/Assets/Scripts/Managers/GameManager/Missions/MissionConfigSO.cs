using UnityEngine;

[CreateAssetMenu(fileName = "MissionConfigSO", menuName = "Scriptable Objects/MissionConfigSO")]
public class MissionConfigSO : ScriptableObject
{
    [field: SerializeField] public FinishMissionSO FinishMissionSO { get; private set; }

    [field: SerializeField, Space(10)] public MissionAssignmentSO MissionAssignment { get; private set; }

    [field: SerializeField, Space(10)] public Missions.Type MissionType { get; private set; }
    [field: SerializeField] public Missions.Params MissionParams { get; private set; }

    [field: SerializeField, Space(10)] public float MissionDuration { get; private set; }
    [field: SerializeField] public float TimerDifficultyMultiplier { get; private set; }
    [field: SerializeField] public float StartMissionDescriptionDuration { get; private set; }
    [field: SerializeField] public int MaxMissionMana { get; private set; }

    [field: SerializeField, Space(10)] public SoundManager.SoundNamesUI SoundNameMissionCompleted { get; private set; }
    [field: SerializeField] public SoundManager.SoundNamesUI SoundNameMissionFailed { get; private set; }
}
