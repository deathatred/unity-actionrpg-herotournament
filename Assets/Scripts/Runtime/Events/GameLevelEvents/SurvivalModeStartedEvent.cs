using Assets.Scripts.Core.Observer;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events.GameLevelEvents
{
    public class SurvivalModeStartedEvent : GameEventBase
    {
        public int Time { get; private set; }

        public SurvivalModeStartedEvent(int time)
        {
            Time = time;
        }
    }
}