using UnityEngine;

[CreateAssetMenu(menuName = "AI/FSM/ActionChase")]
public class ChaseActionSO : ActionSO
{
    public override void Act(EnemyFSMController controller)
    {
        Chase(controller);
    }

    private void Chase(EnemyFSMController controller)
    {
        controller.SetTargetFromChaser();
    }

}
