using UnityEngine;

public class AttackPowerChangedEvent : GameEventBase
{
    public int Amount {  get; private set; }
    public AttackPowerChangedEvent(int amount)
    {
        Amount = amount;
    }
}
