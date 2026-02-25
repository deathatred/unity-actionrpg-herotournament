using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Observer;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events.StatsEvents.NewOnes
{
    public class StatChangedEvent : GameEventBase
    {
        public StatType StatType;
        public int Amount;
        public StatChangedEvent(StatType statType, int amount)
        {
            StatType = statType;
            Amount = amount;
        }
    }
}