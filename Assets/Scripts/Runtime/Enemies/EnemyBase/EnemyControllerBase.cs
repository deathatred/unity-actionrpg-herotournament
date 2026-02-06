using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyControllerBase : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] protected float _rotationSpeed = 8f;
    [SerializeField] private float _baseSpeed;
    public void RotateTowardsTarget(Transform target)
    {
        Vector3 directionToPlayer = target.position - this.transform.position;
        directionToPlayer.y = 0;
        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            this.transform.rotation = Quaternion.Slerp(
                this.transform.rotation,
                targetRotation,
                _rotationSpeed * Time.deltaTime
            );
        }
    }
    public void SetDestination(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);
    }
    public void StopMovement()
    {
        _navMeshAgent.ResetPath();
    }
    public NavMeshAgent GetNavMeshAgent()
    {
        return _navMeshAgent;
    }
    public float GetBaseSpeed()
    {
        return _baseSpeed;
    }
    public float GetCurrentSpeed()
    {
        return _navMeshAgent.speed;
    }
    public void SetSpeed(float speed)
    {
        _navMeshAgent.speed = speed;
    }
}
