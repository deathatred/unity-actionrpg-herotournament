using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Observer;
using UnityEngine;

namespace Assets.Scripts.Runtime.UI.UIEvents
{
    public class ClassSelectedEvent : GameEventBase
    {
        public PlayerClass PlayerClass { get; private set; }
        public ClassSelectedEvent(PlayerClass playerClass)
        {
            PlayerClass = playerClass;
        }
    }
}