using UnityEngine;

public class PointMoveTarget : IMoveTarget
{
    private readonly Vector3 _point;

    public PointMoveTarget(Vector3 point)
    {
        _point = point;
    }

    public Vector3 GetPosition()
    {
        return _point;
    }
}
