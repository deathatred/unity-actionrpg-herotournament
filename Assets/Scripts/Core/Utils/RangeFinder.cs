using UnityEngine;

public static class RangeFinder
{
    public static bool IsClose(Transform player, Transform target, float stopRange)
    {
        Vector3 dir = player.position - target.position;
        dir.y = 0;
        return dir.magnitude < stopRange;

    }
}
