using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class NavMeshDebugger : MonoBehaviour
{
    [SerializeField] private float _lineOffset;
    [SerializeField] private Color[] _colors;
    private NavMeshAgent _agent;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        FindAndTestComponents();
        SetRandomColor();
    }

    private void FindAndTestComponents()
    {
        _agent = Tools.GetComponentWithAssertion<NavMeshAgent>(gameObject);
        _lineRenderer = Tools.GetComponentWithAssertion<LineRenderer>(gameObject);
    }

    private void SetRandomColor()
    {
        if (_colors.Length == 0)
        {
            _lineRenderer.material.color = Color.blue;
        }
        else
        {
            int i = Random.Range(0, _colors.Length);
            _lineRenderer.material.color = _colors[i];
        }
    }

    private void Update()
    {
        DrawPath();
    }

    private void DrawPath()
    {
        if (_agent.hasPath)
        {
            _lineRenderer.positionCount = _agent.path.corners.Length;
            var tmpPos = _agent.path.corners;
            for (int i = 0; i < tmpPos.Length; i++)
            {
                tmpPos[i].y = _agent.path.corners[i].y + _lineOffset;
            }
            _lineRenderer.SetPositions(tmpPos);
            _lineRenderer.enabled = true;
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }

}
