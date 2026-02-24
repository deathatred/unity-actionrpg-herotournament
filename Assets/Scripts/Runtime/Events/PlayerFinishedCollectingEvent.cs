using Assets.Scripts.Core.Observer;
using UnityEngine;

public class PlayerFinishedCollectingEvent : GameEventBase
{
    public ItemInstance Item;
    public PlayerFinishedCollectingEvent(ItemInstance item)
    {
        Item = item;
    }
}
