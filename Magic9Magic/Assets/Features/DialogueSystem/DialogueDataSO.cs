using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueData")]
public class DialogueDataSO : ScriptableObject
{
    [field: SerializeField] public ReplicaStruct[] Replicas { get; private set; }
    [field: SerializeField] public ReplicaStruct[] ReplicasUkraine { get; private set; }
    [field: SerializeField] public ReplicaStruct[] ReplicasRussian { get; private set; }
}

[System.Serializable]
public struct ReplicaStruct
{
    public string ActorName;
    public SoundManager.SoundNamesDialogues SoundName;
    public float Duration;
    [TextArea(5, 10)] public string Text;
}
