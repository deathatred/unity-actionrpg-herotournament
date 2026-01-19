using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SkeletonStateMachine : SummonStateMachine
{
    [Inject] private Transform _playerTransform;
    [SerializeField] private SummonHealthSystem _healthSystem;
    [SerializeField] private SkeletonAnimation _animation;
    [SerializeField] private SkeletonController _controller;
    private Dictionary<SkeletonState, ISummonState> _states;

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void Awake()
    {
        _states = new Dictionary<SkeletonState, ISummonState>
        {
           { SkeletonState.Idle, new SkeletonIdleState(_animation,this,_controller, _playerTransform) },
           { SkeletonState.Moving, new SkeletonMovingState(this,_controller,_animation,_playerTransform) },
           { SkeletonState.Attacking, new SkeletonAttackState(_controller,this, _animation) },
           { SkeletonState.Dead, new SkeletonDeadState(_animation,_controller) }
        };
        ChangeState(SkeletonState.Idle);
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void SubscribeToEvents()
    {
        _healthSystem.OnDeath += EnterDeathState;
    }
    private void EnterDeathState(object sender, EventArgs args)
    {
        ChangeState(SkeletonState.Dead);
        Destroy(this.gameObject,5f);
    }
    private void UnsubscribeFromEvents()
    {
        _healthSystem.OnDeath -= EnterDeathState;
    }
    public void ChangeState(SkeletonState state)
    {
        ChangeState(_states[state]);
    }
}
