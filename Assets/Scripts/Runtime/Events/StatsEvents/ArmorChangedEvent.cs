using Assets.Scripts.Core.Observer;
using UnityEngine;

public class ArmorChangedEvent : GameEventBase
{
    public int Amount { get; private set; }
    public ArmorChangedEvent(int amount)
    {
        Amount = amount;
    }
}
