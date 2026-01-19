using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;

public class SkeletonAttackState : ISummonState
{
    private SkeletonController _controller;
    private SkeletonStateMachine _fsm;
    private SkeletonAnimation _animation;
    private Transform _attackTarget;
    private float _attackRange = 1.5f;
    private float _attackCooldown = 2f;
    private CancellationTokenSource _attackCts = new();
    private bool _isAttacking = false;
    private IHealthSystem _enemyHealth;
    private float _nextRepathTime = 0f;
    private  float _repathCooldown = 0.4f;
    public SkeletonAttackState(SkeletonController controller, SkeletonStateMachine fsm, SkeletonAnimation animation)
    {
        _controller = controller;
        _fsm = fsm;
        _animation = animation;
    }

    public void Enter()
    {
        _attackCts = new CancellationTokenSource();
        _attackTarget = _controller.AttackTarget;
        _enemyHealth = _attackTarget.GetComponent<IHealthSystem>();
    }

    public void Exit()
    {
        CancelAttacking();
    }

    public void Update()
    {
        if (_attackTarget != null)
        {
            _controller.RotateTowardsTarget(_attackTarget);
        }
        HandleAttackBehavior();
    }
    public virtual void HandleAttackBehavior()
    {
        if (_attackTarget == null || _enemyHealth.IsDead)
        {
            _fsm.ChangeState(SkeletonState.Idle);
            return;
        }
        

        float distanceToEnemy = Vector3.Distance(_controller.transform.position, _attackTarget.position);
        if (distanceToEnemy >= _attackRange+0.2f)
        {
            _animation.SetIsMovingTrue();
            HandleApproach();
            CancelAttacking();
            _isAttacking = false;
        }
        else if (!_isAttacking)
        {
            AttackLoop().Forget();
            _isAttacking = true;
        }
    }
    private void HandleApproach()
    {
        _animation.SetIsMovingTrue();

        if (Time.time >= _nextRepathTime)
        {
            Vector3 directionToTarget = (_controller.transform.position - _attackTarget.position).normalized;
            Vector3 pos = _attackTarget.position + directionToTarget * _attackRange;

            _controller.SetMoveTarget(pos);
            _nextRepathTime = Time.time + _repathCooldown;
        }

        _controller.RotateTowardsTarget(_attackTarget);
    }
    private void AttackTarget()
    {
        
        _animation.SetIsMovingFalse();
        _animation.TriggerAttack();
    }
    private void CancelAttacking()
    {
        _attackCts?.Cancel();
        _attackCts.Dispose();
        _attackCts = new CancellationTokenSource();
    }
    private async UniTask AttackLoop()
    { 
        CancelAttacking();
   
        while (!_attackCts.IsCancellationRequested)
        {
            if (_attackTarget == null || _enemyHealth.IsDead)
                break;

            float dist = Vector3.Distance(_controller.transform.position, _attackTarget.position);

            int random = Random.Range(0, 2);
            _animation.SetAttackModifier(random);
            AttackTarget();
            _isAttacking = true;

            await UniTask.WaitForSeconds(_attackCooldown, cancellationToken: _attackCts.Token);
        }
    }
}
