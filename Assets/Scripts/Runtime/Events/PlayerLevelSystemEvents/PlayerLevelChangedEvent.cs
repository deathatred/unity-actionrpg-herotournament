using Assets.Scripts.Core.Observer;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events.PlayerLevelSystemEvents
{
    public class PlayerLevelChangedEvent : GameEventBase
    {
        public int Level { get; private set; }
        public PlayerLevelChangedEvent(int level)
        {
            Level = level;
        }
    }
}