using UnityEngine;

public class CanvasChangedEvent : GameEventBase
{
    public int CanvasId { get; private set;}
    public CanvasChangedEvent(int canvasId)
    {
        CanvasId = canvasId;
    }
}
