using Assets.Scripts.Core.Observer;
using UnityEngine;

public class SurvivalModeStartedEvent : GameEventBase
{
    public int Time { get; private set; }

    public SurvivalModeStartedEvent(int time)
    {
        Time = time;
    }
}
