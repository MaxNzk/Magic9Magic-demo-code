using UnityEngine;

public abstract class ActionSO : ScriptableObject
{
    public abstract void Act(EnemyFSMController controller);
    [SerializeField, TextArea] private string _description;
}
