using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Utils;
using Assets.Scripts.Runtime.Enemies.EnemyBase;
using Assets.Scripts.Runtime.SOScripts.EnemiesSO;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Runtime.Enemies.EnemyBase.EnemyBaseStateMachine.BaseStates
{
    public abstract class EnemyAttackingStateBase<TStateMachine, TController, TAnimator> : IEnemyState
        where TStateMachine : EnemyStateMachine
        where TController : EnemyControllerBase
        where TAnimator : EnemyAnimationBase
    {
        public EnemyStateMachine EnemyFsm => _fsm;

        protected readonly TStateMachine _fsm;
        protected readonly TController _controller;
        protected readonly TAnimator _animator;
        protected readonly EnemyData _data;
        protected readonly EnemyTargetDetector _detector;

        protected CancellationTokenSource _cts;
        protected bool _isAttacking;
        protected float _timeSinceLastSeenTarget;
        protected UniTask _attackTask;
        protected EnemyDataSO _dataSO;
        protected Transform _currentTarget;
        protected EnemyAttackingStateBase(
            TStateMachine fsm,
            TController controller,
            TAnimator animator,
            EnemyData data,
            EnemyTargetDetector detector)
        {
            _fsm = fsm;
            _controller = controller;
            _animator = animator;
            _data = data;
            _detector = detector;
        }

        public virtual void Enter()
        {
            _cts = new CancellationTokenSource();
            _isAttacking = false;
            _dataSO = _data.GetEnemyData();
        }

        public virtual void Exit()
        {
            _cts.Cancel();
            _cts?.Dispose();
            _cts = null;
            _controller.StopMovement();
            _animator.SetIsMovingFalse();
            _isAttacking = false;
        }
        public virtual void Update()
        {

            if (!TryGetTarget(out Transform targetTransform, out Vector3 targetPos))
            {
                return;
            }
            float distance = Vector3.Distance(_controller.transform.position, targetPos);

            if (HandleLostSight(targetTransform, distance))
            {
                return;
            }

            bool inAttackRange = distance <= _dataSO.AttackRange;

            if (inAttackRange)
            {
                HandleAttack(targetTransform);
            }
            else
            {
                HandleMoveToTarget(targetPos);
            }
        }
        protected bool TryGetTarget(out Transform targetTransform, out Vector3 targetPos)
        {
            targetTransform = null;
            targetPos = Vector3.zero;

            ITargetable target = _detector.GetCurrentTarget();

            if (target == null || target.HealthSystem.IsDead)
            {
                OnLostSight();
                return false;
            }

            targetTransform = target.Transform;
            targetPos = targetTransform.position;
            return true;
        }
        protected bool CanSeeTarget(Transform targetTransform)
        {
            return DetectionHelper.CanSeeTarget(
                _controller.transform,
                targetTransform,
                _dataSO.ViewDistance,
                _dataSO.ViewAngle,
                DetectionHelper.DetectionDefaultOffset
            );
        }
        protected bool IsInCloseRange(Transform targetTransform)
        {
            return DetectionHelper.InCloseRange(
                _controller.transform,
                targetTransform,
                _dataSO.CloseRangeTrigger
            );
        }
        protected bool ShouldAttack(bool canSee, bool inCloseRange, float distance)
        {
            if (!(canSee || inCloseRange))
                return false;

            return distance <= _dataSO.AttackRange;
        }
        protected bool HandleLostSight(Transform targetTransform, float currentDistance)
        {
            if (currentDistance > _data.GetEnemyData().LeashRange)
            {
                _timeSinceLastSeenTarget += Time.deltaTime;

                if (_timeSinceLastSeenTarget > _dataSO.LoseSightTime)
                {
                    _timeSinceLastSeenTarget = 0f;
                    OnLostSight();
                    return true;
                }
            }
            else
            {
                _timeSinceLastSeenTarget = 0f;
            }
            return false;
        }
        protected void HandleAttack(Transform targetTransform)
        {
            _controller.StopMovement();
            _animator.SetIsMovingFalse();
            _controller.RotateTowardsTarget(targetTransform);
            if (CanStartBaseAttack())
            {
                if (!_isAttacking && _attackTask.Status != UniTaskStatus.Pending)
                {
                    _isAttacking = true;
                    _attackTask = StartAttackingAsync();
                }
            }
        }
        protected void HandleMoveToTarget(Vector3 targetPos)
        {
            if (_isAttacking)
            {
                _isAttacking = false;
                if (_cts != null)
                {
                    CancelAttack();

                    _attackTask = default;
                }
            }
            if (CanStartBaseAttack())
            {
                _controller.SetDestination(targetPos);
                _animator.SetIsMovingTrue();
            }
            else
            {
                _controller.StopMovement();
                _animator.SetIsMovingFalse();
            }
        }
        protected void CancelAttack()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
            _cts = new CancellationTokenSource();
            _attackTask = default;
            SetIsAttackingFalse();
        }
        protected void SetIsAttackingFalse()
        {
            _isAttacking = false;
        }
        protected virtual bool CanStartBaseAttack()
        {
            return true;
        }
        protected abstract void OnLostSight();

        protected virtual async UniTask StartAttackingAsync()
        {
            var attackCooldown = _dataSO.AttackCooldown;
            try
            {
                while (!_cts.IsCancellationRequested)
                {
                    if (_animator == null) return;
                    _animator.TriggerAttack();
                    await UniTask.WaitForSeconds(attackCooldown, cancellationToken: _cts.Token);
                }
            }
            catch (OperationCanceledException)
            {

            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

    }
}