using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Observer;
using Assets.Scripts.Core.Structs;
using Assets.Scripts.Core.Utils;
using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Events;
using Assets.Scripts.Runtime.MoveTargets;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts.Runtime.Player.FSM
{
    public class PlayerCollectingState : PlayerStateBase
    {
        private readonly PlayerController _playerController;
        private readonly PlayerInputHandler _playerInput;
        private readonly PlayerInteractions _playerInteractions;
        private readonly PlayerInventory _playerInventory;
        private readonly PlayerAnimations _playerAnimations;
        private readonly EventBus _eventBus;

        private const float CollectStopDistance = 1.8f;
        private bool _isCollecting;
        private CancellationTokenSource _rotateCts;

        public PlayerCollectingState(PlayerStateMachine fsm,
         PlayerController controller,
         PlayerInputHandler input,
         PlayerInteractions playerInteractions,
         EventBus eventBus,
         PlayerAnimations playerAnimations,
         PlayerInventory playerInventory) : base(fsm)
        {
            _playerInput = input;
            _playerController = controller;
            _playerInteractions = playerInteractions;
            _eventBus = eventBus;
            _playerAnimations = playerAnimations;
            _playerInventory = playerInventory;
        }

        public override void Enter()
        {
            SubscribeToEvents();
            _rotateCts = new CancellationTokenSource();
            StateType = PlayerState.Collecting;
            SetNewTarget();
        }
        public override void Update()
        {
            HandleCollecting();
        }
        public override void Exit()
        {
            _rotateCts?.Cancel();
            _rotateCts.Dispose();
            StopCollecting();
            UnsubscribeFromEvents();
        }
        private void HandleCollecting()
        {
            if (_playerInput.UserTouching)
            {
                bool groundTargeted = _playerInteractions.IsTargetingGround(_playerInput.PointerPos);
                if (groundTargeted)
                {
                    _fsm.ChangeState(PlayerState.Moving);
                    return;
                }
            }
            if (_playerInteractions.GetCurrentItemTarget() == null) return;
            var itemIsInCollectibleRange = RangeFinder.IsClose(_playerController.transform,
                _playerInteractions.GetCurrentItemTarget().transform, CollectStopDistance);
            if (itemIsInCollectibleRange && !_isCollecting)
            {
                _playerController.RotateToTargetAsync(_playerInteractions.GetCurrentItemTarget().transform.position,
                    _rotateCts.Token).Forget();
                StartCollecting();
            }
            else if (!itemIsInCollectibleRange && _isCollecting)
            {
                StopCollecting();
            }
        }
        private void SetNewTargetSubscriber(TargetChangedEvent e)
        {
            SetNewTarget();
        }
        private void SetNewTarget()
        {
            StopCollecting();
            if (_playerInteractions.GetCurrentItemTarget() == null)
            {
                return;
            }
            _playerController.MoveTo(new MoveCommand
            {
                Target = new TransformMoveTarget(_playerInteractions.GetCurrentItemTarget().transform),
                StopRange = CollectStopDistance,
                RotateTowardsTarget = true
            });
            _playerAnimations.SetIsMovingTrue();
        }
        private void CollectionFinished(PlayerFinishedCollectingEvent e)
        {
            _fsm.ChangeState(PlayerState.Idle);
        }
        private void SubscribeToEvents()
        {
            _eventBus.Subscribe<TargetChangedEvent>(SetNewTargetSubscriber);
            _eventBus.Subscribe<PlayerFinishedCollectingEvent>(CollectionFinished);
        }
        private void UnsubscribeFromEvents()
        {
            _eventBus.Unsubscribe<TargetChangedEvent>(SetNewTargetSubscriber);
            _eventBus.Unsubscribe<PlayerFinishedCollectingEvent>(CollectionFinished);
        }
        private void StartCollecting()
        {
            _isCollecting = true;
            _playerAnimations.SetIsMovingFalse();
            _playerAnimations.SetIsCollectingTrue();
        }

        private void StopCollecting()
        {
            _isCollecting = false;
            _playerAnimations.SetIsCollectingFalse();
        }
    }
}