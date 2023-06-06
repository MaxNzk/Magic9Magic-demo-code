using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSMController : MonoBehaviour
{
    private EnemyStatsSO _enemyStatsSO;
    
    [Space(10)]
    public StateSO CurrentState;
    public List<Transform> WayPointList;
    [HideInInspector] public float LookRanges;
    [HideInInspector] public float AttackRanges;
    [HideInInspector] public int NextWayPoint;
    [HideInInspector] public NavMeshAgent _agent;
    [HideInInspector] public Transform ChaseTarget;
    [HideInInspector] public Transform Target;
    [HideInInspector] public bool IsReachedTarget;

    public void SetTargetFromWayPointList()
    {
        Target = WayPointList[NextWayPoint];

        if (transform.rotation.eulerAngles.x == 90) // Goblin and Wolf have not correct rotation. It is not clear why.
            transform.Rotate(new Vector3(-90, 0, 0));
    }

    public void SetTargetFromChaser()
    {
        Target = ChaseTarget;
    }

    public void SetAttackState(bool isActive)
    {
        IsReachedTarget = isActive;
    }

    private void Start()
    {
        _enemyStatsSO = Tools.GetComponentWithAssertion<EnemyStats>(gameObject).StatsSO;
        _agent = Tools.GetComponentWithAssertion<NavMeshAgent>(gameObject);
        
        Target = WayPointList[NextWayPoint];
        LookRanges = _enemyStatsSO.LookRanges;
        AttackRanges = _enemyStatsSO.StoppingDistance;
    }

    private void Update()
    {
        CurrentState.UpdateState(this);
    }

    private void OnDrawGizmos()
    {
        if (CurrentState != null)
        {
            Gizmos.color = CurrentState.SceneGizmoColor;
            Gizmos.DrawWireSphere(transform.position, 2f);
        }
    }

    public void TransitionToState(StateSO nextState)
    {
        CurrentState = nextState;
    }

}
