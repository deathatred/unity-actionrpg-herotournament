using Assets.Scripts.Core.Observer;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events.PlayerLevelSystemEvents
{
    public class PlayerGainedXpEvent : GameEventBase
    {
        public int CurrentXp { get; private set; }
        public int MaxXp { get; private set; }
        public PlayerGainedXpEvent(int currentXp, int maxXp)
        {
            CurrentXp = currentXp;
            MaxXp = maxXp;
        }
    }
}