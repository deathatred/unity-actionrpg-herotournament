using Assets.Scripts.Runtime.Enemies.EnemyBase.EnemyBaseStateMachine;
using Assets.Scripts.Runtime.Enemies.EnemyBase.EnemyBaseStateMachine.BaseStates;
using UnityEngine;

namespace Assets.Scripts.Runtime.Enemies.Golem.GolemStateMachine.States
{
    public class GolemDeadState : EnemyDeadStateBase
    {
        public GolemDeadState(EnemyStateMachine fsm) : base(fsm)
        {
        }

    }
}