using UnityEngine;

namespace Assets.Scripts.Runtime.Enemies.EnemyBase.EnemyBaseStateMachine.BaseStates
{
    public class EnemyDeadStateBase : IEnemyState
    {
        public EnemyStateMachine EnemyFsm => _fsm;
        private EnemyStateMachine _fsm;
        public EnemyDeadStateBase(EnemyStateMachine fsm)
        {
            _fsm = fsm;
        }

        public void Enter()
        {
            if (_fsm == null)
            {
                return;
            }
            Object.Destroy(_fsm.gameObject, 5f);
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }
    }
}