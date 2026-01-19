using UnityEngine;

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
            Debug.Log("FSM IF THIS == NULL IM GAY");
            return;
        }
        GameObject.Destroy(_fsm.gameObject, 5f);
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }
}
