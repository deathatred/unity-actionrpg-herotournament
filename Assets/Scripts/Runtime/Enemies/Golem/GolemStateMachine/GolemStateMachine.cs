using System.Collections.Generic;
using UnityEngine;

public class GolemStateMachine : EnemyStateMachine
{
    [SerializeField] private EnemyHealthSystem _healthSystem;
    [SerializeField] private GolemAnimation _animation;
    [SerializeField] private GolemController _controller;
    [SerializeField] private EnemyData _data;
    [SerializeField] private EnemyTargetDetector _detector;
    private Dictionary<GolemState, IEnemyState> _states;
    private void OnEnable()
    {
        _healthSystem.OnDeath += HealthSystemOnDeath;
    }

    private void Awake()
    {
        _states = new Dictionary<GolemState, IEnemyState>
        {
           { GolemState.Idle, new GolemIdleState(this, _healthSystem, _detector,_data) },
           { GolemState.Attacking, new GolemAttackingState(this,_controller,_animation,_data, _detector) },
           { GolemState.Dead, new EnemyDeadStateBase(this) }
        };
        ChangeState(GolemState.Idle);
    }
    private void OnDisable()
    {
        _healthSystem.OnDeath -= HealthSystemOnDeath;
        CurrentState.Exit();
    }
    public void ChangeState(GolemState state)
    {
        ChangeState(_states[state]);
    }
    private void HealthSystemOnDeath(object sender, System.EventArgs e)
    {
        ChangeState(GolemState.Dead);
    }

    public override void GoToAttackState()
    {
       ChangeState(GolemState.Attacking);
    }
}
