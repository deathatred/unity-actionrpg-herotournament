using System;
using UnityEngine;

namespace Assets.Scripts.Runtime.Enemies.EyeOfCthulhu.EyeOfCthulhuStateMachine.States
{
    [Serializable]
    public enum EyeOfCthulhuState
    {
        Patroling,
        Attacking,
        Idling,
        Alert,
        Dead
    }
}