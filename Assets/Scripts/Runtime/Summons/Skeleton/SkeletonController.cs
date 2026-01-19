using UnityEngine;
using UnityEngine.AI;

public class SkeletonController : SummonControllerBase
{
    public NavMeshAgent Agent { get; private set; }
    private void Awake()
    {
        Agent = _agent;
        _agent.avoidancePriority = Random.Range(40, 70);
    }
}
