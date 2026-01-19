using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class MimicAttackingState : EnemyAttackingStateBase<MimicStateMachine, MimicController, MimicAnimation>
{
    public MimicAttackingState(MimicStateMachine fsm, MimicController controller,
        MimicAnimation animator, EnemyData data, EnemyTargetDetector detector) 
        : base(fsm, controller, animator, data,detector)
    {
    }

    protected override void OnLostSight()
    {
        _fsm.ChangeState(MimicState.Hidden);
    }
}
