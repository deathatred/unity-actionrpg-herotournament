using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerIdleState : PlayerStateBase
{
    private EventBus _eventBus;
    private readonly PlayerController _playerController;
    private readonly PlayerStateMachine _playerStateMachine;
    private readonly PlayerInputHandler _playerInput;
    private readonly PlayerInteractions _playerInteractions;
    public PlayerIdleState(PlayerStateMachine fsm,
        PlayerController controller,
        PlayerInputHandler input,
        PlayerInteractions playerInteractions,
        EventBus eventBus) : base(fsm)
    {
        _playerInput = input;
        _playerController = controller;
        _playerInteractions = playerInteractions;
        _eventBus = eventBus;
    }

    public PlayerStateMachine StateMachine => _playerStateMachine;
    public override void Enter()
    {
        StateType = PlayerState.Idle;
    }
    public override void Update()
    {
        HandleIdling();
    }
    public override void Exit()
    {
        _playerController.Stop();
    }
    private void HandleIdling()
    {
        if (_playerInput.UserTouching)
        {
            if (_playerInteractions.IsTargetingGround(_playerInput.PointerPos))
            {
                _fsm.ChangeState(PlayerState.Moving);
            }
        }
    }
}
