using UnityEngine;

public class PlayerLevelChangedEvent : GameEventBase
{
    public int Level { get; private set; }
    public PlayerLevelChangedEvent(int level)
    {
        this.Level = level;
    }
}
