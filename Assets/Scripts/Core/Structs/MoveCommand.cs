using UnityEngine;

public struct MoveCommand
{
    public IMoveTarget Target;
    public float StopRange;
    public bool RotateTowardsTarget;
}
