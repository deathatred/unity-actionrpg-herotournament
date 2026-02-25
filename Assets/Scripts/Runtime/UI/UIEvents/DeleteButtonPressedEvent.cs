using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.Items;
using System.Diagnostics.Contracts;
using UnityEngine;

namespace Assets.Scripts.Runtime.UI.UIEvents
{
    public class DeleteButtonPressedEvent : GameEventBase
    {
        public ItemInstance Item { get; private set; }
        public DeleteButtonPressedEvent(ItemInstance item)
        {
            Item = item;
        }
    }
}