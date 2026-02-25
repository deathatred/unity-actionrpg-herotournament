using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Utils;
using Assets.Scripts.Runtime.Enemies.EnemyBase;
using Assets.Scripts.Runtime.Enemies.EnemyBase.EnemyBaseStateMachine;
using Assets.Scripts.Runtime.Enemies.EyeOfCthulhu;
using Assets.Scripts.Runtime.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Runtime.Enemies.EyeOfCthulhu.EyeOfCthulhuStateMachine.States
{
    public class EyeOfCthulhuPatrolState : IEnemyState
    {
        public EnemyStateMachine EnemyFsm => _fsm;
        private GameManager _gameManager;
        private EyeOfCthulhuStateMachine _fsm;
        private EyeOfCthulhuController _controller;
        private EyeOfCthulhuAnimator _animator;
        private List<Transform> _patrolPoints;
        private EnemyTargetDetector _detector;
        private float _reachThreshold = 0.5f;
        private const float ViewAngle = 100f;
        private const float ViewDistance = 15f;
        private const float CloseRangeTrigger = 4f;

        public EyeOfCthulhuPatrolState(List<Transform> patrolPoints,
            GameManager gameManager,
            EyeOfCthulhuStateMachine fsm,
            EyeOfCthulhuController controller,
            EyeOfCthulhuAnimator animator,
            EnemyTargetDetector detector)
        {
            _patrolPoints = patrolPoints;
            _gameManager = gameManager;
            _fsm = fsm;
            _controller = controller;
            _animator = animator;
            _detector = detector;
        }

        public void Enter()
        {
            _animator.SetIsMovingTrue();
            if (_patrolPoints.Count > 0)
                _controller.SetDestination(_patrolPoints[_fsm.CurrentPatrolIndex].position);
        }

        public void Exit()
        {
            _animator.SetIsMovingFalse();
            _controller.StopMovement();
        }

        public void Update()
        {
            HandlePauseReaction();
            DetectTarget();
            Patrol();
        }
        private void DetectTarget()
        {
            Transform targetTransform = null;
            if (_detector.TryFindClosestTarget(out ITargetable target))
            {
                targetTransform = target.Transform;

                bool canSeePlayer = DetectionHelper.CanSeeTarget(_controller.transform,
                    targetTransform, ViewDistance, ViewAngle,
                    DetectionHelper.DetectionDefaultOffset);
                bool inCloseRange = DetectionHelper.InCloseRange(_controller.transform, targetTransform, CloseRangeTrigger);
                if (canSeePlayer || inCloseRange)
                {
                    _fsm.ChangeState(EyeOfCthulhuState.Attacking);
                }
            }
        }
        private void Patrol()
        {
            if (_patrolPoints.Count == 0) return;

            if (Vector3.Distance(_controller.transform.position, _patrolPoints[_fsm.CurrentPatrolIndex].position) < _reachThreshold)
            {
                _fsm.CurrentPatrolIndex = (_fsm.CurrentPatrolIndex + 1) % _patrolPoints.Count;
                _controller.SetDestination(_patrolPoints[_fsm.CurrentPatrolIndex].position);
            }
        }
        private void HandlePauseReaction()
        {
            if (_gameManager.IsPaused)
            {
                _controller.GetNavMeshAgent().isStopped = true;
                return;
            }
            else if (!_gameManager.IsPaused && _controller.GetNavMeshAgent().isStopped)
            {
                _controller.GetNavMeshAgent().isStopped = false;
            }
        }
    }
}