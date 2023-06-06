using UnityEngine;

public abstract class DecisionSO : ScriptableObject
{
    public abstract bool Decide (EnemyFSMController controller);
    [SerializeField, TextArea] private string _description;
}
