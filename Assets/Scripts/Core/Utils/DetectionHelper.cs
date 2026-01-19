using UnityEngine;

public static class DetectionHelper
{
    public static int obstacleMask = LayerMask.GetMask("Obstacles");
    public static readonly Vector3 DetectionDefaultOffset = Vector3.up * 1.2f;
    public static bool CanSeeTarget(
     Transform detector,
     Transform target,
     float viewDistance,
     float viewAngle,
     Vector3 raycastOffset)
    {
        if (target == null) return false;

        Vector3 origin = detector.position + raycastOffset;

        Collider targetCol = target.GetComponentInChildren<Collider>();
        Vector3 closestPoint = targetCol.ClosestPoint(origin);

        Vector3 direction = closestPoint - origin;
        float distance = direction.magnitude;

        if (distance > viewDistance)
            return false;

        float angle = Vector3.Angle(detector.forward, direction);
        if (angle > viewAngle * 0.5f)
            return false;

        if (Physics.Raycast(origin, direction.normalized, out RaycastHit hit, distance, obstacleMask))
        {
            return hit.transform == target || hit.transform.IsChildOf(target);
        }

        return true;
    }


    public static bool InCloseRange(Transform detector, Transform target, float triggerRange) 
    {
        float distance = Vector3.Distance(detector.position, target.position);
        bool inCloseRange = distance < triggerRange;
        return inCloseRange;
    }
}