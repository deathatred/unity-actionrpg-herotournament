using Assets.Scripts.Runtime.SOScripts.EnemiesSO;
using System;
using UnityEngine;

namespace Assets.Scripts.Runtime.LevelsLogic
{
    [Serializable]
    public class EnemySpawnData
    {
        public EnemyDataSO EnemyData;
        public Transform SpawnPoint;
        public int SpawnIndex;
    }
}