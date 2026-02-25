using Assets.Scripts.Core.Enums;
using Assets.Scripts.Runtime.SpellsContext;
using NUnit.Framework;
using System;
using UnityEngine;

namespace Assets.Scripts.Core.Data
{
    [Serializable]
    public class PlayerSaveData
    {
        public PlayerClass PlayerClass;
        public int CurrentLevel;
        public int ExpAmount;
        public int ExpRequired;
        public int LevelPointsAmount;
        public int TalentPointsAmount;
        public int CurrentHealth;
        public int CurrentMana;
        public StatsSaveData StatsData;
        public TalentSaveData[] LearnedTalentsIds;
        public InventoryItemsSaveData[] InventoryItems;
        public BonusEffect[] BonusEffects;
        public Vector3 Position;
    }
}