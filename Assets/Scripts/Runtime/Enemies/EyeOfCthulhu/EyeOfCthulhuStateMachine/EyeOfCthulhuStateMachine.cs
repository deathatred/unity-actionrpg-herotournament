using Assets.Scripts.Runtime.Enemies.EnemyBase;
using Assets.Scripts.Runtime.Enemies.EnemyBase.EnemyBaseStateMachine;
using Assets.Scripts.Runtime.Enemies.EnemyBase.EnemyBaseStateMachine.BaseStates;
using Assets.Scripts.Runtime.Enemies.EyeOfCthulhu;
using Assets.Scripts.Runtime.Enemies.EyeOfCthulhu.EyeOfCthulhuStateMachine.States;
using Assets.Scripts.Runtime.Managers;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Runtime.Enemies.EyeOfCthulhu.EyeOfCthulhuStateMachine
{
    public class EyeOfCthulhuStateMachine : EnemyStateMachine
    {
        [Inject] private GameManager _gameManager;
        [SerializeField] private EyeOfCthulhuController _controller;
        [SerializeField] private EnemyHealthSystem _healthSystem;
        [SerializeField] private EyeOfCthulhuAnimator _animator;
        [SerializeField] private EnemyData _data;
        [SerializeField] private EnemyTargetDetector _detector;
        public EyeOfCthulhuState State;
        private Dictionary<EyeOfCthulhuState, IEnemyState> _states;

        public int CurrentPatrolIndex { get; set; } = 0;
        private void OnEnable()
        {
            _healthSystem.OnDeath += HealthSystemOnDeath;
        }
        private void Awake()
        {
            _states = new Dictionary<EyeOfCthulhuState, IEnemyState>
        {
           { EyeOfCthulhuState.Patroling, new EyeOfCthulhuPatrolState(_gameManager.CurrentLevel.GetPatrolPoints(),_gameManager,
           this,_controller,
           _animator,
           _detector) },
           { EyeOfCthulhuState.Attacking, new EyeOfCthulhuAttackingState(this,_controller,_animator,
           _data, _detector) },
           { EyeOfCthulhuState.Dead, new EnemyDeadStateBase(this) },
        };
            ChangeState(EyeOfCthulhuState.Patroling);
        }
        private void OnDisable()
        {
            _healthSystem.OnDeath -= HealthSystemOnDeath;
        }
        private void HealthSystemOnDeath(object sender, System.EventArgs e)
        {
            ChangeState(EyeOfCthulhuState.Dead);
        }
        public void ChangeState(EyeOfCthulhuState state)
        {
            State = state;
            ChangeState(_states[state]);
        }

        public override void GoToAttackState()
        {
            if (State == EyeOfCthulhuState.Attacking)
            {
                return;
            }
            ChangeState(EyeOfCthulhuState.Attacking);
        }
    }
}