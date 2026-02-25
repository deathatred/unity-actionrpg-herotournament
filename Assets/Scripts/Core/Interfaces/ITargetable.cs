using UnityEngine;

namespace Assets.Scripts.Core.Interfaces
{
    public interface ITargetable
    {
        public Transform Transform { get; }
        public IHealthSystem HealthSystem { get; }
    }
}