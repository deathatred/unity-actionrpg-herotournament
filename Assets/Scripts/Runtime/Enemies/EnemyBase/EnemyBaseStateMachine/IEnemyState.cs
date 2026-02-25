using Assets.Scripts.Runtime.BaseLogic;
using UnityEngine;

namespace Assets.Scripts.Runtime.Enemies.EnemyBase.EnemyBaseStateMachine
{
    public interface IEnemyState : IState
    {
        public EnemyStateMachine EnemyFsm { get; }
    }
}