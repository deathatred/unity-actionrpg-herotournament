using UnityEngine;

public class MoveSpeedChangedEvent : GameEventBase
{
    public int Amount { get; private set; }
    public MoveSpeedChangedEvent(int amount)
    {
        Amount = amount;
    }
}
