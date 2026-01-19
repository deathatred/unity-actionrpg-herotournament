using UnityEngine;
using Zenject;


public class KnightAnimationRelay : PlayerAnimationRelayBase
{   
    [SerializeField] private TrailRenderer[] _bloodTrails;

    public void EnableBloodTrail()
    {
        foreach (var trail in  _bloodTrails)
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
