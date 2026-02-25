using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Observer;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events.StatsEvents.NewOnes
{
    public class AllStatsChangedEvent : GameEventBase
    {
        public Dictionary<StatType, int> Stats;
        public AllStatsChangedEvent(Dictionary<StatType, int> stats)
        {
            Stats = stats;
        }
    }
}