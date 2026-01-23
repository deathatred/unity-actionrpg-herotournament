using UnityEngine;

public class PlayerConfiguredEvent : GameEventBase
{
    public PlayerClass PlayerClass { get; private set; }
    public PlayerConfiguredEvent(PlayerClass playerClass)
    {
        PlayerClass = playerClass;
    }
}
