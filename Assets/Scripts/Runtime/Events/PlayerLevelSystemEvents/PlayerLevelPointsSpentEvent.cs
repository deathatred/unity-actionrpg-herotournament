using UnityEngine;

public class PlayerLevelPointsSpentEvent : GameEventBase
{
    public int Amount { get; private set; }
    public PlayerLevelPointsSpentEvent(int amount)
    {
        Amount = amount;
    }
}
