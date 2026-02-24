using Assets.Scripts.Core.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Core.Structs
{
    public struct MoveCommand
    {
        public IMoveTarget Target;
        public float StopRange;
        public bool RotateTowardsTarget;
    }
}