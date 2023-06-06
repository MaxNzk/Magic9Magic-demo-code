using UnityEngine;

[CreateAssetMenu(menuName = "AI/FSM/DecisionActiveState")]
public class ActiveStateDecisionSO : DecisionSO
{
    public override bool Decide(EnemyFSMController controller)
    {
        return controller.ChaseTarget.gameObject.activeSelf == false;
    }
    
}
