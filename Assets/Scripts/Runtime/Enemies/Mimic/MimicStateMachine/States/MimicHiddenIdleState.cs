using Unity.Android.Gradle.Manifest;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MimicHiddenIdleState : EnemyIdleStateBase<MimicStateMachine, MimicState>
{
    public MimicHiddenIdleState(MimicStateMachine fsm, EnemyHealthSystem healthSystem, EnemyTargetDetector detector, EnemyData data) : base(fsm, healthSystem, detector, data)
    {
    }

    protected override void ChangeToAttackState()
    {
        _fsm.ChangeState(MimicState.Attacking);
    }
}
