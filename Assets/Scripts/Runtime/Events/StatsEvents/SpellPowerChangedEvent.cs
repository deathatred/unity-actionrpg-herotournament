using Assets.Scripts.Core.Observer;
using UnityEngine;

public class SpellPowerChangedEvent : GameEventBase
{
    public int Amount { get; private set; }
    public SpellPowerChangedEvent(int amount)
    {
        Amount = amount;
    }
}
