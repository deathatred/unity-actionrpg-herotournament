using Assets.Scripts.Runtime.Enemies.EnemyBase;
using Assets.Scripts.Runtime.Enemies.EnemyBase.EnemyBaseStateMachine.BaseStates;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Runtime.Enemies.Mimic.MimicStateMachine.States
{
    public class MimicAttackingState : EnemyAttackingStateBase<MimicStateMachine, MimicController, MimicAnimation>
    {
        public MimicAttackingState(MimicStateMachine fsm, MimicController controller,
            MimicAnimation animator, EnemyData data, EnemyTargetDetector detector)
            : base(fsm, controller, animator, data, detector)
        {
        }

        protected override void OnLostSight()
        {
            _fsm.ChangeState(MimicState.Hidden);
        }
    }
}