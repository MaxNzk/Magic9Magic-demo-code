using UnityEngine;

[CreateAssetMenu(menuName = "AI/FSM/ActionAttack")]
public class AttackActionSO : ActionSO
{
    public override void Act(EnemyFSMController controller)
    {
        Attack(controller);
    }

    private void Attack(EnemyFSMController controller)
    {
        #if UNITY_EDITOR
        Debug.DrawRay(controller.transform.position, controller.transform.forward.normalized * controller.AttackRanges, Color.red);
        #endif
        if (Physics.Raycast(controller.transform.position, controller.transform.forward, out RaycastHit hit, controller.AttackRanges) && hit.collider.CompareTag("Player"))
        {
            controller.SetAttackState(true);
        }
        else
        {
            controller.SetAttackState(false);
        }
    }
}
