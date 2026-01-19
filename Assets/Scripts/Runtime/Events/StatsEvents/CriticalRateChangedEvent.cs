using UnityEngine;

public class CriticalRateChangedEvent : GameEventBase
{
    public int Amount { get; private set; }
    public CriticalRateChangedEvent(int amount)
    {
        Amount = amount;
    }
}
