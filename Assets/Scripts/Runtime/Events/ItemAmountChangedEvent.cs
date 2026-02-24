using Assets.Scripts.Core.Observer;
using UnityEngine;

public class ItemAmountChangedEvent : GameEventBase
{
    public int Amount { get; private set; }
    public ItemAmountChangedEvent(int amount)
    {
        Amount = amount;
    }
}
