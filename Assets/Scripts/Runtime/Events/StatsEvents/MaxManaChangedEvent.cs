using Assets.Scripts.Core.Observer;
using UnityEngine;

public class MaxManaChangedEvent : GameEventBase
{
    public int Amount { get; private set; }
    public MaxManaChangedEvent(int amount)
    {
        Amount = amount;
    }
}
