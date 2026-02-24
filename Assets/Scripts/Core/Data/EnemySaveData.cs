using System;
using Unity;
using UnityEngine;

namespace Assets.Scripts.Core.Data
{
    [Serializable]
    public class EnemySaveData
    {
        public string EnemyId;
        public Vector3 Position;
        public int CurrentHealth;
        public bool IsAlive;
    }
}