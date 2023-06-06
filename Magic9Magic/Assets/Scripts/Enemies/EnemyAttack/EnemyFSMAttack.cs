public class EnemyFSMAttack : EnemyAttack
{
    public override bool GetAttackState()
    {
        EnemyFSMController enemyFSMController = Tools.GetComponentWithAssertion<EnemyFSMController>(gameObject);
        return enemyFSMController.IsReachedTarget;
    }
}
