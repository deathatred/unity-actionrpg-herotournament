using Assets.Scripts.Core.Observer;
using UnityEngine;

public class IntellectChangedEvent : GameEventBase
{
    public int Amount { get; private set; } 
    public IntellectChangedEvent(int amount)
    {
        Amount = amount;
    }
}
