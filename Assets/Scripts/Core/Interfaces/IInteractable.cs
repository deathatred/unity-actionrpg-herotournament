using UnityEngine;

namespace Assets.Scripts.Core.Interfaces
{
    public interface IInteractable
    {
        public Transform Transform { get; }
        public void Interact();
    }
}