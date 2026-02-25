using UnityEngine;

namespace Assets.Scripts.Runtime.Player
{
    public class PlayerWallCheck : MonoBehaviour
    {
        [SerializeField] private LayerMask _wallsMask;
        public bool WallCheck(Rigidbody rb, Vector3 moveDir)
        {
            float checkDistance = 0.8f;
            float offset = 0.3f;
            Vector3 normalizedDir = moveDir.normalized;
            Vector3 right = Vector3.Cross(normalizedDir, Vector3.up).normalized;
            Vector3 left = -right;
            Vector3 rayStartCenter = rb.position + Vector3.up * 0.9f;
            Vector3 rayStartFeet = rb.position + Vector3.up * 0.2f;
            if (CastRay(rayStartCenter, normalizedDir, checkDistance)) return true;
            if (CastRay(rayStartFeet, normalizedDir, checkDistance)) return true;
            if (CastRay(rayStartCenter + left * offset, normalizedDir, checkDistance)) return true;
            if (CastRay(rayStartCenter + right * offset, normalizedDir, checkDistance)) return true;
            if (CastRay(rayStartFeet + left * offset, normalizedDir, checkDistance)) return true;
            if (CastRay(rayStartFeet + right * offset, normalizedDir, checkDistance)) return true;
            return false;
        }
        private bool CastRay(Vector3 start, Vector3 dir, float distance)
        {
            return Physics.Raycast(
                start,
                dir,
                distance,
                _wallsMask,
                QueryTriggerInteraction.Ignore
            );
        }
    }
}