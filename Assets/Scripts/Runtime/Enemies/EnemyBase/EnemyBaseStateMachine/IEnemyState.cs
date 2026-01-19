using UnityEngine;

public interface IEnemyState : IState
{
    public EnemyStateMachine EnemyFsm { get; }
}
