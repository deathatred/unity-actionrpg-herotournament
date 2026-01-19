using UnityEngine;

public class BackButtonPressedEvent : GameEventBase
{
   public BackButtonCaller Caller { get; private set; }

    public BackButtonPressedEvent(BackButtonCaller caller)
    {
        this.Caller = caller;
    }
}
