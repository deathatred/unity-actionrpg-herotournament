using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.Items;
using UnityEngine;

namespace Assets.Scripts.Runtime.UI.UIEvents
{
    public class SlotButtonPressedEvent : GameEventBase
    {
        public ItemInstance SlotItem { get; private set; }
        public SlotButtonPressedEvent(ItemInstance slotItem)
        {
            SlotItem = slotItem;
        }
    }
}