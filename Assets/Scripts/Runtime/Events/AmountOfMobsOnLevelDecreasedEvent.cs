using Assets.Scripts.Core.Observer;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events
{
    public class AmountOfMobsOnLevelDecreasedEvent : GameEventBase
    {
        public int AmountOfMobs;
        public AmountOfMobsOnLevelDecreasedEvent(int amountOfMobs)
        {
            AmountOfMobs = amountOfMobs;
        }
    }
}