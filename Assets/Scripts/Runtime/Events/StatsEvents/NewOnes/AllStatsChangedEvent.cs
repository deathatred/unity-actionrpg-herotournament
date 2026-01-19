using System.Collections.Generic;
using UnityEngine;

public class AllStatsChangedEvent : GameEventBase
{
   public Dictionary<StatType, int> Stats;
    public AllStatsChangedEvent(Dictionary<StatType, int> stats)
    {
        Stats = stats;
    }
}
