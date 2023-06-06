using UnityEngine;

[CreateAssetMenu(menuName = "AI/FSM/DecisionLook360")]
public class Look360DecisionSO : DecisionSO
{
    [SerializeField] LayerMask _layerMask;
    [SerializeField] string _targetName = "Player";
    private Collider[] _colliders;
    public override bool Decide(EnemyFSMController controller)
    {
        return Look(controller);
    }

    private bool Look(EnemyFSMController controller)
    {
        Debug.DrawRay(controller.transform.position, controller.transform.forward.normalized * controller.LookRanges, Color.green);
        Debug.DrawRay(controller.transform.position, controller.transform.right.normalized * controller.LookRanges, Color.green);
        Debug.DrawRay(controller.transform.position, controller.transform.forward.normalized * controller.LookRanges * -1, Color.green);
        Debug.DrawRay(controller.transform.position, controller.transform.right.normalized * controller.LookRanges * -1, Color.green);

        _colliders = Physics.OverlapSphere(controller.transform.position, controller.LookRanges, _layerMask);
        for (int i = 0; i < _colliders.Length; i++)
        {
            if (_colliders[i].name == _targetName)
            {
                controller.ChaseTarget = _colliders[i].transform;
                return true;
            }
        }
        return false;
    }

}
