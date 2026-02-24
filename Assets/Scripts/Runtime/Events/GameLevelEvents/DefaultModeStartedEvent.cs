using Assets.Scripts.Core.Observer;
using UnityEngine;

public class DefaultModeStartedEvent : GameEventBase 
{
    public int AmountOfEnemies;
    public DefaultModeStartedEvent(int amountOfEnemies)
    {
        AmountOfEnemies = amountOfEnemies;
    }
}
