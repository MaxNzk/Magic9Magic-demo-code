using UnityEngine;

[CreateAssetMenu(menuName = "AI/FSM/ActionPatrol")]
public class PatrolActionSO : ActionSO
{
    public override void Act(EnemyFSMController controller)
    {
        Patrol(controller);
    }

    private void Patrol(EnemyFSMController controller)
    {
        if (controller._agent.remainingDistance <= controller._agent.stoppingDistance && controller._agent.pathPending == false)
        {
            controller.NextWayPoint = (controller.NextWayPoint + 1) % controller.WayPointList.Count;
            controller.SetTargetFromWayPointList();
        }
    }

}
