using Assets.Scripts.Runtime.SOScripts;
using System;
using UnityEngine;

namespace Assets.Scripts.Runtime.LevelsLogic
{
    [Serializable]
    public class ItemSpawnData
    {
        public InventoryItemSO Item;
        public Transform SpawnPoint;
        public string SpawnIndex;
    }
}