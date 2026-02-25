using Assets.Scripts.Runtime.Enemies.EnemyBase;
using Assets.Scripts.Runtime.Enemies.EnemyBase.EnemyBaseStateMachine.BaseStates;
using UnityEngine;


namespace Assets.Scripts.Runtime.Enemies.Mimic.MimicStateMachine.States
{
    public class MimicHiddenIdleState : EnemyIdleStateBase<MimicStateMachine, MimicState>
    {
        public MimicHiddenIdleState(MimicStateMachine fsm, EnemyHealthSystem healthSystem, EnemyTargetDetector detector, EnemyData data) : base(fsm, healthSystem, detector, data)
        {
        }

        protected override void ChangeToAttackState()
        {
            _fsm.ChangeState(MimicState.Attacking);
        }
    }
}