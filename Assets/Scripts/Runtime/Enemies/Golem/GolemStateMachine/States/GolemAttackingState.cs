using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class GolemAttackingState
    : EnemyAttackingStateBase<GolemStateMachine, GolemController, GolemAnimation>
{
    private float _smashTimer;
    private float _smashTimerMax = 10f;
    private float _smashDuration = 5f;

    private bool _isSmashing;
    private CancellationTokenSource _smashCts;

    public GolemAttackingState(
        GolemStateMachine fsm,
        GolemController controller,
        GolemAnimation animator,
        EnemyData data,
        EnemyTargetDetector detector)
        : base(fsm, controller, animator, data, detector)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _smashTimer = 0f;
        _isSmashing = false;

        _smashCts = new CancellationTokenSource();

        SmashLoop(_smashCts.Token).Forget();
    }

    public override void Exit()
    {
        CancelSmashing();
        base.Exit();
    }
    public override void Update()
    {
        if (_isSmashing)
        {
            return;
        }
        base.Update();
    }

    protected override void OnLostSight()
    {
        Debug.Log("lost sight");
        _fsm.ChangeState(GolemState.Idle);
    }

    private async UniTask SmashLoop(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                _smashTimer += Time.deltaTime;

                if (_smashTimer > _smashTimerMax && !_isSmashing)
                {
                    _isSmashing = true;
                    _smashTimer = 0f;

                    CancelAttack();
                    _controller.StopMovement();
                    _animator.SetSmashIsTrue();

                    await UniTask.WaitForSeconds(_smashDuration, cancellationToken: token);

                    _isSmashing = false;
                    _animator.SetSmashIsFalse();
                }

                await UniTask.Yield(token);
            }
        }
        catch (OperationCanceledException)
        {
            
        }
    }

    protected override bool CanStartBaseAttack()
    {
        if (_isSmashing) return false;

        if (_smashTimer >= _smashTimerMax) return false;

        return true;
    }
    private void CancelSmashing()
    {
        _smashCts?.Cancel();
        _smashCts?.Dispose();
        _smashCts = null;
        _animator.SetSmashIsFalse();
    }
}
