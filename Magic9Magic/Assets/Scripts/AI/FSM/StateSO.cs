using UnityEngine;

[CreateAssetMenu(menuName = "AI/FSM/New State")]
public class StateSO : ScriptableObject
{
    [SerializeField, TextArea] private string _description;
    
    [Space(10)]
    public ActionSO[] ActionSOList;
    public Transition[] transitions;
    public Color SceneGizmoColor = Color.gray;

    public void UpdateState(EnemyFSMController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(EnemyFSMController controller)
    {
        for (int i = 0; i < ActionSOList.Length; i++)
        {
            ActionSOList[i].Act(controller);
        }
    }

    private void CheckTransitions(EnemyFSMController controller)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            if (transitions[i].Decision.Decide(controller))
                controller.TransitionToState(transitions[i].NextState);
        }
    }

}
