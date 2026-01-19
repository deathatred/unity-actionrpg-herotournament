using Cysharp.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

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
