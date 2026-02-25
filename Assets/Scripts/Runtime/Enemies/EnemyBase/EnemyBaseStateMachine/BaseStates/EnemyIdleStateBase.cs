using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Utils;
using Assets.Scripts.Runtime.Enemies.EnemyBase;
using Assets.Scripts.Runtime.SOScripts.EnemiesSO;
using System;
using UnityEngine;

namespace Assets.Scripts.Runtime.Enemies.EnemyBase.EnemyBaseStateMachine.BaseStates
{
    public abstract class EnemyIdleStateBase<TStateMachine, TStateType> : IEnemyState
        where TStateMachine : EnemyStateMachine
        where TStateType : Enum
    {
        public EnemyStateMachine EnemyFsm => _fsm;

        protected readonly TStateMachine _fsm;
        protected readonly EnemyData _data;
        protected readonly EnemyHealthSystem _healthSystem;
        protected readonly EnemyTargetDetector _detector;

        private EnemyDataSO _dataSO;

        public EnemyIdleStateBase(TStateMachine fsm,
            EnemyHealthSystem healthSystem,
            EnemyTargetDetector detector,
            EnemyData data)
        {
            _fsm = fsm;
            _healthSystem = healthSystem;
            _detector = detector;
            _data = data;
        }
        public void Enter()
        {
            _dataSO = _data.GetEnemyData();
        }
        public void Exit()
        {

        }
        public void Update()
        {
            var viewDistance = _dataSO.ViewDistance;
            var viewAngle = _dataSO.ViewAngle;
            if (_detector.TryFindClosestTarget(out ITargetable closestTarget))
            {
                if (DetectionHelper.CanSeeTarget(_fsm.transform, closestTarget.Transform,
                viewDistance, viewAngle,
                DetectionHelper.DetectionDefaultOffset) || DetectionHelper.InCloseRange(_fsm.transform,
                closestTarget.Transform, _dataSO.CloseRangeTrigger))
                {
                    ChangeToAttackState();
                }
            }

        }
        protected abstract void ChangeToAttackState();
    }
}