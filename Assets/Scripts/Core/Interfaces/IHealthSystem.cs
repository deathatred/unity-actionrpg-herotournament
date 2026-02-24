using System;
using UnityEngine;

namespace Assets.Scripts.Core.Interfaces
{
    public interface IHealthSystem
    {
        public int Health { get; }
        public int Armor { get; }
        public bool IsDead { get; }
        public int TakeDamage(int amount);
        public void Heal(int amount);
        public void RestoreHealth(int amount);
        public event Action<int, int> OnHealthChanged;
        public event EventHandler OnDeath;
    }
}