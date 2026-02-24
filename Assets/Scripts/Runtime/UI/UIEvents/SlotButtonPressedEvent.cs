using Assets.Scripts.Core.Observer;
using UnityEngine;

public class SlotButtonPressedEvent : GameEventBase
{
   public ItemInstance SlotItem {  get; private set; }
    public SlotButtonPressedEvent(ItemInstance slotItem)
    {
        SlotItem = slotItem;
    }
}
