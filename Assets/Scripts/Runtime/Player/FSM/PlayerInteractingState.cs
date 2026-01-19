using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class PlayerInteractingState : PlayerStateBase
{
    private const float InteractStopDistance = 1.6f;
    private readonly PlayerController _playerController;
    private readonly PlayerInputHandler _playerInput;
    private readonly PlayerInteractions _playerInteractions;
    private readonly PlayerAnimations _playerAnimations;
    private readonly EventBus _eventBus;

    private bool _isInteracting;
    private CancellationTokenSource _rotateCts;

    public PlayerInteractingState(PlayerStateMachine fsm,
     PlayerController controller,
     PlayerInputHandler input,
     PlayerInteractions playerInteractions,
     EventBus eventBus,
     PlayerAnimations playerAnimations) : base(fsm)
    {
        _playerInput = input;
        _playerController = controller;
        _playerInteractions = playerInteractions;
        _eventBus = eventBus;
        _playerAnimations = playerAnimations;
    }

    public override void Enter()
    {
        _rotateCts = new CancellationTokenSource();
        SubscribeToEvents();
        StateType = PlayerState.Interacting;
        SetNewTarget();
    }
    public override void Update()
    {
        HandleInteracting();
    }
    public override void Exit()
    {
        _rotateCts?.Cancel();
        _rotateCts?.Dispose();
        UnsubscribeFromEvents();
    }
    private void HandleInteracting()
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
        if (_playerInteractions.GetCurrentInteractableTarget() == null) return;
        var interactableIsInRange = RangeFinder.IsClose(_playerController.transform,
            _playerInteractions.GetCurrentInteractableTarget().Transform, InteractStopDistance);
        if (interactableIsInRange && !_isInteracting)
        {
            _playerController.RotateToTargetAsync(_playerInteractions.GetCurrentItemTarget().transform.position,
               _rotateCts.Token).Forget();
            _playerAnimations.SetIsMovingFalse();
            _playerInteractions.GetCurrentInteractableTarget().Interact(); 
            _fsm.ChangeState(PlayerState.Idle);
            _isInteracting = true;
        }
        else if (!interactableIsInRange && _isInteracting)
        {
            _isInteracting = false;
        }
    }
    private void SetNewTargetSubscriber(TargetChangedEvent e)
    {
        SetNewTarget();
    }
    private void SetNewTarget()
    {
        _playerController.MoveTo(new MoveCommand
        {
            Target = new TransformMoveTarget(_playerInteractions.GetCurrentInteractableTarget().Transform),
            StopRange = InteractStopDistance,
            RotateTowardsTarget = true
        });
        _playerAnimations.SetIsMovingTrue();
        _isInteracting = false;
    }

    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<TargetChangedEvent>(SetNewTargetSubscriber);
    }
    private void UnsubscribeFromEvents()
    {
        _eventBus.Unsubscribe<TargetChangedEvent>(SetNewTargetSubscriber);
    }
}
