using UnityEngine;

public class StrenghtChangedEvent : GameEventBase
{
    public int Amount { get; private set; } 
    public StrenghtChangedEvent(int amount)
    {
        Amount = amount;
    }
}
