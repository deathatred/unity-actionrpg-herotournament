using Assets.Scripts.Core.Observer;
using System.Diagnostics.Contracts;
using UnityEngine;

public class DeleteButtonPressedEvent : GameEventBase
{
    public ItemInstance Item { get ; private set; }
    public DeleteButtonPressedEvent(ItemInstance item)
    {
        Item = item;
    }
}
