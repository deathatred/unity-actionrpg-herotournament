using UnityEngine;

namespace Assets.Scripts.Runtime.Summons.Skeleton.FSM.States
{
    public class SkeletonMovingState : ISummonState
    {
        private SkeletonStateMachine _fsm;
        private SkeletonController _controller;
        private SkeletonAnimation _animation;
        private Transform _player;

        private float _nextRepathTime;
        private const float RepathCooldown = 1f;
        public SkeletonMovingState(SkeletonStateMachine fsm, SkeletonController controller,
            SkeletonAnimation animation,
            Transform player)
        {
            _fsm = fsm;
            _controller = controller;
            _animation = animation;
            _player = player;
        }
        public void Enter()
        {
            PickNewFollowPoint();
            _nextRepathTime = Time.time + RepathCooldown;
            _animation.SetIsMovingTrue();
        }

        public void Exit()
        {
        }
        public void Update()
        {
            HandleEnemyAgrroing();
            HandleMoving();
        }
        private void HandleMoving()
        {
            float distToTarget = Vector3.Distance(_controller.transform.position, _controller.CurrentMoveTarget);

            if (distToTarget <= 1f)
            {
                _fsm.ChangeState(SkeletonState.Idle);
                return;
            }

            float distToPlayer = Vector3.Distance(_controller.transform.position, _player.position);
            if (distToPlayer > _controller.FollowDistance + 0.5f && Time.time >= _nextRepathTime)
            {
                PickNewFollowPoint();
                _nextRepathTime = Time.time + RepathCooldown;
                _animation.SetIsMovingTrue();
            }
        }
        private void HandleEnemyAgrroing()
        {
            Collider[] enemies = Physics.OverlapSphere(_controller.transform.position,
                _controller.AggroRange,
                LayerMask.GetMask("Enemy"));

            if (enemies.Length > 0)
            {
                _controller.SetTarget(enemies[0].transform);
                _fsm.ChangeState(SkeletonState.Attacking);
            }
        }
        private void PickNewFollowPoint()
        {
            float distToPlayer = Vector3.Distance(_controller.transform.position, _player.position);

            Vector3 followPoint;

            if (distToPlayer > 5.0f)
            {
                Vector3 directionFromPlayer = (_controller.transform.position - _player.position).normalized;
                followPoint = _player.position + directionFromPlayer * 1.5f;
            }
            else
            {
                followPoint = GetRandomFollowPoint(_controller.FollowDistance);
            }

            _controller.SetMoveTarget(followPoint);
        }
        private Vector3 GetRandomFollowPoint(float radius)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(
                Mathf.Cos(angle),
                0f,
                Mathf.Sin(angle)
            ) * radius;

            return _player.position + offset;
        }
    }
}