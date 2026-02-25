using UnityEngine;

namespace Assets.Scripts.Runtime.Summons.Skeleton.FSM.States
{
    public class SkeletonDeadState : ISummonState
    {
        private SkeletonAnimation _animation;
        private SkeletonController _controller;
        public SkeletonDeadState(SkeletonAnimation animation, SkeletonController controller)
        {
            _animation = animation;
            _controller = controller;
        }

        public void Enter()
        {
            _controller.ResetPath();
            _animation.SetIsDeadTrue();
        }

        public void Exit()
        {

        }

        public void Update()
        {

        }
    }
}