using UnityEngine;

public class GolemIdleState : EnemyIdleStateBase<GolemStateMachine, GolemState>
{
    public GolemIdleState(GolemStateMachine fsm, EnemyHealthSystem healthSystem, EnemyTargetDetector detector, EnemyData data) : base(fsm, healthSystem, detector, data)
    {
    }

    protected override void ChangeToAttackState()
    {
        _fsm.ChangeState(GolemState.Attacking);
    }
}
