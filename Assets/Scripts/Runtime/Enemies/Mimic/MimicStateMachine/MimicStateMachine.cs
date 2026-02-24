using Cysharp.Threading.Tasks.Triggers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class MimicStateMachine : EnemyStateMachine
{
    [SerializeField] private EnemyHealthSystem _healthSystem;
    [SerializeField] private MimicAnimation _mimicAnimation;
    [SerializeField] private MimicController _controller;
    [SerializeField] private EnemyData _data;
    [SerializeField] private EnemyTargetDetector _detector;
    private Dictionary<MimicState, IEnemyState> _states;
    private MimicState _currentStateEnum;
    private void OnEnable()
    {
        _healthSystem.OnDeath += HealthSystemOnDeath;
    }
    private void Awake()
    {
        _states = new Dictionary<MimicState, IEnemyState>
        {
           { MimicState.Hidden, new MimicHiddenIdleState(this, _healthSystem, _detector,_data) },
           { MimicState.Attacking, new MimicAttackingState(this, _controller,
           _mimicAnimation,_data, _detector) },
           { MimicState.Dead, new EnemyDeadStateBase(this) },
        };
        ChangeState(MimicState.Hidden);
    }
    private void OnDisable()
    {
        _healthSystem.OnDeath -= HealthSystemOnDeath;
    }
    private void HealthSystemOnDeath(object sender, System.EventArgs e)
    {
        ChangeState(MimicState.Dead);
    }
    public override void GoToAttackState()
    {
        if (_currentStateEnum == MimicState.Attacking)
        {
            return;
        }
        ChangeState(MimicState.Attacking);
    }
    public void ChangeState(MimicState state)
    {
        _currentStateEnum = state;
        ChangeState(_states[state]);
    }
}