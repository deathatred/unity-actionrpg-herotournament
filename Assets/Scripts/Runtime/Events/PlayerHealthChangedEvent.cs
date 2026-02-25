using Assets.Scripts.Core.Observer;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events
{
    public class PlayerHealthChangedEvent : GameEventBase
    {
        public int MaxHealth { get; private set; }
        public int CurrentHealth { get; private set; }
        public PlayerHealthChangedEvent(int maxHealth, int currentHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
        }
    }
}