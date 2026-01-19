using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpellCastingState : PlayerStateBase
{
    private readonly PlayerController _playerController;
    private readonly PlayerInputHandler _playerInput;
    private readonly PlayerInteractions _playerInteractions;
    private readonly PlayerSpellCasting _playerSpellCasting;

    private readonly PlayerAnimations _playerAnimations;
    private readonly EventBus _eventBus;

    private bool _autoAttack = true;
    public PlayerSpellCastingState(PlayerStateMachine fsm,
     PlayerController controller,
     PlayerInputHandler input,
     PlayerInteractions playerInteractions,
     EventBus eventBus,
     PlayerAnimations playerAnimations,
     PlayerSpellCasting playerSpellCasting) : base(fsm)
    {
        _playerInput = input;
        _playerController = controller;
        _playerInteractions = playerInteractions;
        _eventBus = eventBus;
        _playerAnimations = playerAnimations;
        _playerSpellCasting = playerSpellCasting;
    }
    private bool CanCastWhileMoving =>
    _playerSpellCasting.LastCastSpell != null &&
    _playerSpellCasting.LastCastSpell.CanBeCastWhileMoving;
    public override void Enter()
    {
        UpdateMovementVisuals();
        SubscribeToEvents();
    }
    public override void Exit()
    {
        _playerAnimations.SetIsMovingFalse();
        UnsubscribeFromEvents();
    }
    public override void Update()
    {
        UpdateMovementVisuals();
        HandleMovingIfNeeded();
    }
    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<PlayerSpellCastEnded>(EndSpellCasting);
    }
    private void UnsubscribeFromEvents()
    {
        _eventBus.Unsubscribe<PlayerSpellCastEnded>(EndSpellCasting);
    }
    private void EndSpellCasting(PlayerSpellCastEnded e)
    {
        if (_playerInteractions.GetCurrentEnemyTarget() != null && _autoAttack)
        {
            _fsm.ChangeState(PlayerState.Attacking);
        }
        else if (_playerController.IsMoving)
        {
            _fsm.ChangeState(PlayerState.Moving);
            return;
        }
        else 
            _fsm.ChangeState(_fsm.LastState);
    }
    private void UpdateMovementVisuals()
    {
        bool shouldMove = _playerController.IsMoving && CanCastWhileMoving;
        if (shouldMove)
        {
            _playerAnimations.SetIsMovingTrue();
        }
        else
        {
            _playerAnimations.SetIsMovingFalse();
        }
    }
    private void HandleMovingIfNeeded()
    {
        if (CanCastWhileMoving)
        {
            if (_playerInput.UserTouching)
            {
                if (_playerInteractions.IsTargetingGround(_playerInput.PointerPos))
                MoveCharacter();
            }
        }
    }
    private void MoveCharacter()
    {
        if (_playerInput.UserTouching)
        {
            if (MousePoint.Instance.TryGetPointerWorldPosition(_playerInput.PointerPos, out var worldPos))
            {
                _playerController.MoveTo(new MoveCommand
                {
                    Target = new PointMoveTarget(worldPos),
                    StopRange = 0,
                    RotateTowardsTarget = true
                });
            }
            else
            {
                return;
            }
        }
    }
}
