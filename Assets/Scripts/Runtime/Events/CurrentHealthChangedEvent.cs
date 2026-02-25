using Assets.Scripts.Core.Observer;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events
{
    public class CurrentHealthChangedEvent : GameEventBase
    {
        public int Amount { get; private set; }
        public CurrentHealthChangedEvent(int amount)
        {
            Amount = amount;
        }
    }
}