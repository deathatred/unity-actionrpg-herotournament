using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.Items;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events
{
    public class ItemAddedToInventoryEvent : GameEventBase
    {
        public bool Stacked { get; private set; }
        public ItemInstance Item { get; private set; }
        public ItemAddedToInventoryEvent(bool stacked, ItemInstance item)
        {
            Stacked = stacked;
            Item = item;
        }
    }
}