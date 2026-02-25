using Assets.Scripts.Core.Observer;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events.GameLevelEvents
{
    public class DefaultModeStartedEvent : GameEventBase
    {
        public int AmountOfEnemies;
        public DefaultModeStartedEvent(int amountOfEnemies)
        {
            AmountOfEnemies = amountOfEnemies;
        }
    }
}