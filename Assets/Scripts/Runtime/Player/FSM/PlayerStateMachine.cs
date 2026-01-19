using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerStateMachine : MonoBehaviour
{
    [Inject] private EventBus _eventBus;
    [Inject] private PlayerStateFactory _stateFactory;
    [Inject] private PlayerInteractions _playerInteractions;
    [Inject] private PlayerInputHandler _playerInput;
    private PlayerStateBase _currentState;
    private bool _isTouched;
    public PlayerState CurrentState { get; private set; }
    public PlayerState LastState { get; private set; }
   
    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void Awake()
    {
        HandleStartingState();
    }
    private void Update()
    {

        HandleGlobalTransitions();
        _currentState.Update();
        print(CurrentState);
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void HandleStartingState()
    {
        ChangeState(PlayerState.Idle);
    }
    private void HandleGlobalTransitions()
    {
        if (_playerInput.UserTouching && !_isTouched)
        {
            if (_playerInteractions.HandleEnemyInteraction(_playerInput.PointerPos))
            {
                _isTouched = true;
                return;
            }
            if (_playerInteractions.HandleInteractableInteraction(_playerInput.PointerPos))
            {
                _isTouched = true;
                ChangeState(PlayerState.Interacting);
                return;
            }
            if (_playerInteractions.HandleItemInteraction(_playerInput.PointerPos))
            {
                _isTouched = true;
                ChangeState(PlayerState.Collecting);
                return;
            }
        }
        else if (!_playerInput.UserTouching && _isTouched)
        {
            _isTouched = false; return;
        }
    }
    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<PlayerSpellCastedEvent>(ChangeToSpellCastState);
        _eventBus.Subscribe<SameEnemyClickedEvent>(ChangeToAttackState);
    }
    private void UnsubscribeFromEvents()
    {
        _eventBus.Unsubscribe<PlayerSpellCastedEvent>(ChangeToSpellCastState);
        _eventBus.Unsubscribe<SameEnemyClickedEvent>(ChangeToAttackState);
    }
    private void ChangeToSpellCastState(PlayerSpellCastedEvent e)
    {
        ChangeState(PlayerState.SpellCasting);
    }
    private void ChangeToAttackState(SameEnemyClickedEvent e)
    {
        if (CurrentState != PlayerState.SpellCasting)
        {
            ChangeState(PlayerState.Attacking);
        }
    }
    public void ChangeState(PlayerState state)
    {
        if (_currentState != null)
        {
            LastState = _currentState.StateType;
            _currentState.Exit();
        }
        CurrentState = state;
        _currentState = _stateFactory.CreatePlayerState(state);
        _currentState.Enter();
    }
}
