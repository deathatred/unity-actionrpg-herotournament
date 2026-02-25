using Assets.Scripts.Core.Observer;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events
{
    public class ItemAmountChangedEvent : GameEventBase
    {
        public int Amount { get; private set; }
        public ItemAmountChangedEvent(int amount)
        {
            Amount = amount;
        }
    }
}