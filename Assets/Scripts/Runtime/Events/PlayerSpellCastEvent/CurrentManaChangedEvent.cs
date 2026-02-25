using Assets.Scripts.Core.Observer;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events.PlayerSpellCastEvent
{
    public class CurrentManaChangedEvent : GameEventBase
    {
        public int Amount { get; private set; }
        public CurrentManaChangedEvent(int amount)
        {
            Amount = amount;
        }
    }
}