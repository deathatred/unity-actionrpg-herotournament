using Assets.Scripts.Core.Observer;
using UnityEngine;

public class AmountOfMobsOnLevelDecreasedEvent : GameEventBase
{
    public int AmountOfMobs;
    public AmountOfMobsOnLevelDecreasedEvent(int amountOfMobs)
    {
        AmountOfMobs = amountOfMobs;
    }
}
