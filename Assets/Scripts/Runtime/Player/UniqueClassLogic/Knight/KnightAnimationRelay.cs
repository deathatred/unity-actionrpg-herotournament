using UnityEngine;
using Zenject;

namespace Assets.Scripts.Runtime.Player.UniqueClassLogic.Knight
{
    public class KnightAnimationRelay : PlayerAnimationRelayBase
    {
        [SerializeField] private TrailRenderer[] _bloodTrails;

        public void EnableBloodTrail()
        {
            foreach (var trail in _bloodTrails)
            {
                trail.emitting = true;
            }
        }
        public void DisableBloodTrail()
        {
            foreach (var trail in _bloodTrails)
            {
                trail.emitting = false;
            }
        }
    }
}