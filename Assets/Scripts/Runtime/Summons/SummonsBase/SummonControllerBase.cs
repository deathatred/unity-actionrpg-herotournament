using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Assets.Scripts.Runtime.Summons.SummonsBase
{
    public class SummonControllerBase : MonoBehaviour
    {
        [Inject] private Transform _player;
        [SerializeField] protected NavMeshAgent _agent;
        public Vector3 CurrentMoveTarget => _currentMoveTarget;
        public float AggroRange { get; private set; } = 8f;
        public float FollowDistance { get; private set; } = 2.5f;
        public Transform AttackTarget { get; private set; }

        private float _rotationSpeed = 8f;
        private Transform _enemyTarget;

        private Vector3 _currentMoveTarget;
        protected virtual void Update()
        {

        }
        public void RotateTowardsTarget(Transform target)
        {
            Vector3 directionToPlayer = target.position - transform.position;
            directionToPlayer.y = 0;
            if (directionToPlayer != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    _rotationSpeed * Time.deltaTime
                );
            }
        }
        public void SetMoveTarget(Vector3 point)
        {
            _currentMoveTarget = point;
            _agent.SetDestination(point);
        }
        public void ResetPath()
        {
            _agent.ResetPath();
        }
        public virtual void SetTarget(Transform target)
        {
            AttackTarget = target;
        }
        public virtual void ClearTarget()
        {
            AttackTarget = null;
        }
    }
}