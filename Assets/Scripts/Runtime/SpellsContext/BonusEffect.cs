using Assets.Scripts.Core.Enums;
using System;
using UnityEngine;

namespace Assets.Scripts.Runtime.SpellsContext
{
    [Serializable]
    public class BonusEffect
    {
        public string Name;
        public StatType StatType;
        public int Amount;
        public float RemainingTime;

        public BonusEffect(string name, StatType statType, int amount, float remainingTime)
        {
            Name = name;
            StatType = statType;
            Amount = amount;
            RemainingTime = remainingTime;
        }
    }
}