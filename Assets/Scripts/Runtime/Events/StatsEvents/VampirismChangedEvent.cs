using UnityEngine;

public class VampirismChangedEvent : GameEventBase
{
    public int Amount { get; private set; }
    public VampirismChangedEvent(int amount)
    {
        Amount = amount;
    }
}
