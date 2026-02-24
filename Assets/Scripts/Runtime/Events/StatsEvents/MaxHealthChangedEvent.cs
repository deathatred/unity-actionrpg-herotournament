using Assets.Scripts.Core.Observer;
using UnityEngine;

public class MaxHealthChangedEvent : GameEventBase
{
    public int Amount { get; private set; }
    public MaxHealthChangedEvent(int amount)
    {
        Amount = amount;
    }
}
