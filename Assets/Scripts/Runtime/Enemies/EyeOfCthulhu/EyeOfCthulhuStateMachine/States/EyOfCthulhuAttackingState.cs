using Assets.Scripts.Runtime.Enemies.EnemyBase;
using Assets.Scripts.Runtime.Enemies.EnemyBase.EnemyBaseStateMachine.BaseStates;
using Assets.Scripts.Runtime.Enemies.EyeOfCthulhu;
using Assets.Scripts.Runtime.Enemies.EyeOfCthulhu.EyeOfCthulhuStateMachine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Runtime.Enemies.EyeOfCthulhu.EyeOfCthulhuStateMachine.States
{
    public class EyeOfCthulhuAttackingState : EnemyAttackingStateBase<EyeOfCthulhuStateMachine, EyeOfCthulhuController,
        EyeOfCthulhuAnimator>
    {
        public EyeOfCthulhuAttackingState(EyeOfCthulhuStateMachine fsm,
            EyeOfCthulhuController controller, EyeOfCthulhuAnimator animator,
            EnemyData data, EnemyTargetDetector detector) : base(fsm, controller, animator, data, detector)
        {
        }
        protected override void OnLostSight()
        {
            _fsm.ChangeState(EyeOfCthulhuState.Patroling);
        }
    }
}