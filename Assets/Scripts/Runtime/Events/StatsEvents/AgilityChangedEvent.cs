using UnityEngine;

public class AgilityChangedEvent : GameEventBase
{
    public int Amount { get; private set; } 
    public AgilityChangedEvent(int amount)
    {
        Amount = amount;
    }
}
