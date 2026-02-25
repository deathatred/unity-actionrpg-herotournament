using Assets.Scripts.Core.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Runtime.MoveTargets
{
    public class TransformMoveTarget : IMoveTarget
    {
        private readonly Transform _transform;
        public TransformMoveTarget(Transform transform)
        {
            _transform = transform;
        }

        public Vector3 GetPosition()
        {
            if (_transform == null)
                return Vector3.zero;

            return _transform.position;
        }
    }
}