using Assets.Scripts.Core.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Runtime.Summons.Skeleton.FSM.States
{
    public class SkeletonIdleState : ISummonState
    {
        private SkeletonStateMachine _fsm;
        private Transform _player;
        private SkeletonAnimation _animation;
        private SkeletonController _controller;

        public SkeletonIdleState(SkeletonAnimation animation,
            SkeletonStateMachine fsm,
            SkeletonController controller,
            Transform player)
        {
            _fsm = fsm;
            _controller = controller;
            _animation = animation;
            _player = player;
        }
        public void Enter()
        {
            _animation.SetIsMovingFalse();
        }

        public void Exit()
        {

        }

        public void Update()
        {
            HandleEnemyAgrroing();
            HandleIdling();
        }
        private void HandleIdling()
        {
            float dist = Vector3.Distance(_controller.transform.position, _player.position);

            if (dist > _controller.FollowDistance + 0.5f)
            {
                _fsm.ChangeState(SkeletonState.Moving);
            }
        }
        private void HandleEnemyAgrroing()
        {
            Collider[] enemies = Physics.OverlapSphere(_controller.transform.position,
                _controller.AggroRange,
                LayerMask.GetMask("Enemy"));

            if (enemies.Length > 0)
            {
                if (enemies[0].GetComponent<IHealthSystem>().IsDead)
                {
                    return;
                }
                _controller.SetTarget(enemies[0].transform);
                _fsm.ChangeState(SkeletonState.Attacking);
            }
        }
    }
}