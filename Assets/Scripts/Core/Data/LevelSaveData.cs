using System;
using UnityEngine;

namespace Assets.Scripts.Core.Data
{
    [Serializable]
    public class LevelSaveData
    {
        public int LevelIndex;
        public bool IsSurvivalMode;
        public int SurvivalTimeLeft;
        public int TimeToNextSpawn;
        public int AmountOfEnemiesLeft;
        public EnemySaveData[] Enemies;
        public string[] ItemsSceneIDs;
    }
}