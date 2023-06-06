using UnityEngine;

[CreateAssetMenu(menuName = "AI/FSM/DecisionLook")]
public class LookDecisionSO : DecisionSO
{
    public override bool Decide(EnemyFSMController controller)
    {
        return Look(controller);
    }

    private bool Look(EnemyFSMController controller)
    {
        #if UNITY_EDITOR
        Debug.DrawRay(controller.transform.position, controller.transform.forward.normalized * controller.LookRanges, Color.green);
        #endif
        if (Physics.SphereCast(controller.transform.position, controller.LookRanges / 2f, controller.transform.forward, out RaycastHit hit, controller.LookRanges) 
            && hit.collider.CompareTag("Player"))
        {
            controller.ChaseTarget = hit.transform;
            return true;
        }
        return false;
    }

}
