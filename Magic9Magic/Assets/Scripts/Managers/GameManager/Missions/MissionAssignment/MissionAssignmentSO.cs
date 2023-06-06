using UnityEngine;

[CreateAssetMenu(fileName = "MissionAssignment", menuName = "Scriptable Objects/MissionAssignmentSO")]
public class MissionAssignmentSO : ScriptableObject
{
    [SerializeField] private string missionAssignmentEn;
    [SerializeField] private string missionAssignmentUa;
    [SerializeField] private string missionAssignmentRu;

    public string GetMissionAssignment(string lang)
    {
        switch (lang)
        {
            case "English": return missionAssignmentEn;
            case "Ukraine": return missionAssignmentUa;
            case "Russian": return missionAssignmentRu;
            default: return missionAssignmentEn;
        }
    }
}
