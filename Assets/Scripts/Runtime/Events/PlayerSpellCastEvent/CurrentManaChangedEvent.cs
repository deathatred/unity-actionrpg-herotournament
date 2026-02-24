using Assets.Scripts.Core.Observer;
using UnityEngine;

public class CurrentManaChangedEvent : GameEventBase
{
    public int Amount { get; private set; }
    public CurrentManaChangedEvent(int amount)
    {
        Amount = amount;
    }
}
