using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.Player;
using TMPro;
using UnityEngine;

public class PlayerMoveToSpellTargetState : PlayerStateBase
{
    private readonly EventBus _eventBus;
    private readonly PlayerController _controller;
    private readonly PlayerSpellCasting _spellCasting;
    private readonly PlayerInteractions _interaction;
    private readonly PlayerAnimations _animation;

    private SpellSO _queuedSpell;
    private bool _eventFired;
    public PlayerMoveToSpellTargetState(
        EventBus eventBus,
        PlayerSpellCasting spellCasting,
        PlayerController controller,
        PlayerInteractions interactions,
        PlayerAnimations animation,
        PlayerStateMachine fsm) : base(fsm)
    {
        _eventBus = eventBus;
        _controller = controller;
        _spellCasting = spellCasting;
        _interaction = interactions;
        _animation = animation;
    }
    public override void Enter()
    {
        StateType = PlayerState.MoveToSpellTarget;
        var target = _interaction.GetCurrentEnemyTarget();
        if (target == null)
        {
            _fsm.ChangeState(PlayerState.Idle);
            return;
        }

        _queuedSpell = _spellCasting.GetQueuedSpell();
        if (_queuedSpell == null)
        {
            _fsm.ChangeState(PlayerState.Idle);
            return;
        }
        _controller.MoveTo(new MoveCommand
        {
            Target = new TransformMoveTarget(target.transform),
            StopRange = _queuedSpell.Range,
            RotateTowardsTarget = true
        });
        _animation.SetIsMovingTrue();
    }
    public override void Exit()
    {
        _animation.SetIsMovingFalse();
    }
    public override void Update()
    {
        if (_interaction.GetCurrentEnemyTarget()  == null)
        {
            _fsm.ChangeState(PlayerState.Idle);
            return;
        }
        if (_queuedSpell == null)
        {
            _fsm.ChangeState(PlayerState.Idle);
            return;
        }
        if (_spellCasting.IsInSpellRange(_queuedSpell) && !_eventFired)
        {
            _eventBus.Publish(new ReachedSpellTarget());    
            _eventFired = true;
        }
    }
}
