using UnityEngine;

public class PlayerMovingState : PlayerStateBase
{
    private readonly EventBus _eventBus;
    private readonly PlayerController _playerController;
    private readonly PlayerInputHandler _playerInput;
    private readonly PlayerInteractions _playerInteractions;
    private readonly PlayerAnimations _playerAnimations;

    public PlayerMovingState(PlayerStateMachine fsm,
        PlayerController controller,
        PlayerInputHandler input,
        PlayerInteractions playerInteractions,
        PlayerAnimations playerAnimations,
        EventBus eventBus) : base(fsm)
    {
        _playerInput = input;
        _playerController = controller;
        _playerInteractions = playerInteractions;
        _playerAnimations = playerAnimations;
        _eventBus = eventBus;
    }

    public override void Enter()
    {
        MoveCharacter();
        StateType = PlayerState.Moving;
        _playerAnimations.SetIsMovingTrue();
    }

    public override void Exit()
    {
        _playerAnimations.SetIsMovingFalse();
    }

    public override void Update()
    {
        if (_playerInput.UserTouching && _playerInteractions.GetCurrentEnemyTarget() == null)
        {
            MoveCharacter();
        }
        if (!_playerController.IsMoving)
        {
            _fsm.ChangeState(PlayerState.Idle);
        }
    }
    private void MoveCharacter()
    {
        if (_playerInput.UserTouching)
        {
            if (MousePoint.Instance.TryGetPointerWorldPosition(_playerInput.PointerPos,
                 out Vector3 worldPos))
            {
                _playerController.MoveTo(new MoveCommand
                {
                    Target = new PointMoveTarget(worldPos),
                    StopRange = 0f,
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
