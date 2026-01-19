using UnityEngine;

public class EnemyStateMachine : StateMachineBase<IEnemyState>
{
    private void OnDisable()
    {
        CurrentState.Exit();
    }
}
