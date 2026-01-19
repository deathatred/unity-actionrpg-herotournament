using UnityEngine;

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
