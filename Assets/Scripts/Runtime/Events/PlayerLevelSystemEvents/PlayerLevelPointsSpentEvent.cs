using Assets.Scripts.Core.Observer;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events.PlayerLevelSystemEvents
{
    public class PlayerLevelPointsSpentEvent : GameEventBase
    {
        public int Amount { get; private set; }
        public PlayerLevelPointsSpentEvent(int amount)
        {
            Amount = amount;
        }
    }
}