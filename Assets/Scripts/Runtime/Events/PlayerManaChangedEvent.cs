using UnityEngine;

public class PlayerManaChangedEvent : GameEventBase
{
    public int CurrentMana { get; private set; }
    public int MaxMana { get; private set; }
    public PlayerManaChangedEvent(int currentMana, int maxMana)
    {  
        CurrentMana = currentMana;
        MaxMana = maxMana;
    }
}
