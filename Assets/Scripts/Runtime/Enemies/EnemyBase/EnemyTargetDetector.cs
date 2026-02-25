using Assets.Scripts.Core.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Runtime.Enemies.EnemyBase
{
    public class EnemyTargetDetector : MonoBehaviour
    {
        [SerializeField] private EnemyControllerBase _controller;
        [SerializeField] private EnemyData _data;
        [SerializeField] private LayerMask _targetMask;

        private ITargetable _currentTarget;

        public bool TryFindClosestTarget(out ITargetable closestTarget)
        {
            closestTarget = null;
            Collider[] hits = Physics.OverlapSphere(
                _controller.transform.position,
                _data.GetEnemyData().LeashRange,
                _targetMask
            );
            if (hits.Length == 0)
            {
                _currentTarget = null;
                return false;
            }
            ITargetable closest = null;
            float bestDist = float.MaxValue;

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<ITargetable>(out var target))
                {
                    if (target.HealthSystem.IsDead)
                        continue;

                    float dist = Vector3.Distance(_controller.transform.position, target.Transform.position);

                    if (dist < bestDist)
                    {
                        bestDist = dist;
                        closest = target;
                    }
                }
            }
            if (closest != null)
            {
                closestTarget = closest;
                _currentTarget = closestTarget;
                return true;
            }
            _currentTarget = null;
            return false;
        }
        public ITargetable GetCurrentTarget()
        {
            return _currentTarget;
        }
    }
}