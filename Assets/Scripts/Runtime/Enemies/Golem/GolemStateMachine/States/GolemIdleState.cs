using Assets.Scripts.Runtime.Enemies.EnemyBase;
using Assets.Scripts.Runtime.Enemies.EnemyBase.EnemyBaseStateMachine.BaseStates;
using Assets.Scripts.Runtime.Enemies.Golem.GolemStateMachine;
using UnityEngine;

namespace Assets.Scripts.Runtime.Enemies.Golem.GolemStateMachine.States
{
    public class GolemIdleState : EnemyIdleStateBase<GolemStateMachine, GolemState>
    {
        public GolemIdleState(GolemStateMachine fsm, EnemyHealthSystem healthSystem, EnemyTargetDetector detector, EnemyData data) : base(fsm, healthSystem, detector, data)
        {
        }

        protected override void ChangeToAttackState()
        {
            _fsm.ChangeState(GolemState.Attacking);
        }
    }
}