using UnityEngine;

public class EnemyFSMMovement : EnemyMovement
{
    private EnemyFSMController _enemyFSMController;

    protected override void FindAndTestComponents()
    {
        base.FindAndTestComponents();
        _enemyFSMController = Tools.GetComponentWithAssertion<EnemyFSMController>(gameObject);
    }
    
    protected override Vector3 GetDestination()
    {
        return _enemyFSMController.Target.position;
    }

    protected override Transform LookAtTarget()
    {
        return _enemyFSMController.Target;
    }
}
