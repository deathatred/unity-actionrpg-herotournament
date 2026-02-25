using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.Items;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events
{
    public class PlayerFinishedCollectingEvent : GameEventBase
    {
        public ItemInstance Item;
        public PlayerFinishedCollectingEvent(ItemInstance item)
        {
            Item = item;
        }
    }
}