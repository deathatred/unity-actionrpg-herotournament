using Assets.Scripts.Core.Observer;
using UnityEngine;

public class AttackSpeedChangedEvent : GameEventBase
{
    public int Amount { get; private set; }
    public AttackSpeedChangedEvent(int amount)
    {
        Amount = amount;
    }
}
