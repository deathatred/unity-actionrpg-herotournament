using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Observer;
using UnityEngine;

namespace Assets.Scripts.Runtime.UI.UIEvents
{
    public class BackButtonPressedEvent : GameEventBase
    {
        public BackButtonCaller Caller { get; private set; }

        public BackButtonPressedEvent(BackButtonCaller caller)
        {
            Caller = caller;
        }
    }
}