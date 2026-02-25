using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Observer;
using Assets.Scripts.Core.Structs;
using Assets.Scripts.Core.Utils;
using Assets.Scripts.Runtime.Enemies.EnemyBase;
using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Events;
using Assets.Scripts.Runtime.MoveTargets;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Runtime.Player.FSM
{
    public class PlayerAttackState : PlayerStateBase
    {
        private readonly PlayerController _playerController;
        private readonly PlayerInputHandler _playerInput;
        private readonly PlayerInteractions _playerInteractions;
        private readonly PlayerAttackSystem _playerAttack;
        private readonly PlayerAnimations _playerAnimations;
        private readonly EventBus _eventBus;

        private bool _isAttacking = false;
        private EnemyHealthSystem _lastTarget;
        private CancellationTokenSource _rotationCts;
        public PlayerAttackState(PlayerStateMachine fsm,
         PlayerController controller,
         PlayerInputHandler input,
         PlayerInteractions playerInteractions,
         EventBus eventBus,
            PlayerAttackSystem playerAttack,
            PlayerAnimations playerAnimations) : base(fsm)
        {
            _playerInput = input;
            _playerController = controller;
            _playerInteractions = playerInteractions;
            _playerAttack = playerAttack;
            _eventBus = eventBus;
            _playerAnimations = playerAnimations;
        }

        public override void Enter()
        {
            SubscribeToEvents();
            _rotationCts = new CancellationTokenSource();
            StateType = PlayerState.Attacking;
            SetNewTarget();
        }
        public override void Update()
        {
            HandleAttacking();
        }
        public override void Exit()
        {
            _rotationCts.Cancel();
            _rotationCts.Dispose();
            _playerAttack.CancelAttacking();
            UnsubscribeFromEvents();
        }
        private void HandleAttacking()
        {
            if (_playerInput.UserTouching)
            {
                bool groundTargeted = _playerInteractions.IsTargetingGround(_playerInput.PointerPos);
                if (groundTargeted)
                {
                    _fsm.ChangeState(PlayerState.Moving);
                }
            }

            var currentTarget = _playerInteractions.GetCurrentEnemyTarget();
            if (currentTarget == null || currentTarget.IsDead)
            {
                _fsm.ChangeState(PlayerState.Idle);
                return;
            }
            var enemyIsInRange = RangeFinder.IsClose(_playerController.transform, currentTarget.transform, _playerAttack.GetAttackRange());
            if (enemyIsInRange && !_isAttacking)
            {
                _playerAnimations.SetIsMovingFalse();
                _playerController.RotateToTargetAsync(currentTarget.transform.position, _rotationCts.Token).Forget();
                _playerAttack.MeleeAttackAsync(currentTarget).Forget();
                _isAttacking = true;
            }
            else if (!enemyIsInRange && _isAttacking)
            {
                _playerAttack.CancelAttacking();
                _isAttacking = false;
            }
            if (_playerInteractions.GetCurrentEnemyTarget() != null)
            {
                if (_playerInteractions.GetCurrentEnemyTarget().IsDead)
                {
                    _fsm.ChangeState(PlayerState.Idle);
                }
            }
        }
        private void SetNewTarget()
        {
            var current = _playerInteractions.GetCurrentEnemyTarget();

            if (current == null)
            {
                _lastTarget = null;
                _fsm.ChangeState(PlayerState.Idle);
                return;
            }
            if (current == _lastTarget)
                return;

            _lastTarget = current;

            _playerController.MoveTo(new MoveCommand
            {
                Target = new TransformMoveTarget(current.transform),
                StopRange = _playerAttack.GetAttackRange(),
                RotateTowardsTarget = true
            });

            _playerAnimations.SetIsMovingTrue();

            _isAttacking = false;
            _playerAttack.CancelAttacking();

        }
        private void SubscribeToEvents()
        {
            _eventBus.Subscribe<PlayerStartedAttackEvent>(StartAttack);
        }
        private void UnsubscribeFromEvents()
        {
            _eventBus.Unsubscribe<PlayerStartedAttackEvent>(StartAttack);
        }
        private void StartAttack(GameEventBase e)
        {
            _playerAnimations.StartAttackingAnimation();
        }
    }
}