using Assets.Scripts.Core.Observer;
using UnityEngine;

public class PlayerGainedXpEvent : GameEventBase
{
    public int CurrentXp { get; private set; }
    public int MaxXp { get; private set; }
    public PlayerGainedXpEvent(int currentXp, int maxXp)
    {
        CurrentXp = currentXp;
        MaxXp = maxXp;
    }
}
