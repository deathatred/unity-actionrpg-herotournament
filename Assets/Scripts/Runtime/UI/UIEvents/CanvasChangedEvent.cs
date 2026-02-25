using Assets.Scripts.Core.Observer;
using UnityEngine;

namespace Assets.Scripts.Runtime.UI.UIEvents
{
    public class CanvasChangedEvent : GameEventBase
    {
        public int CanvasId { get; private set; }
        public CanvasChangedEvent(int canvasId)
        {
            CanvasId = canvasId;
        }
    }
}