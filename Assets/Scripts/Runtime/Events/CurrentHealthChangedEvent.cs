using UnityEngine;

public class CurrentHealthChangedEvent : GameEventBase
{
    public int Amount { get;  private set; }
    public CurrentHealthChangedEvent(int amount)
    {
        Amount = amount;
    }
}
