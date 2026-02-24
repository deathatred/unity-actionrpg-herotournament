using UnityEngine;

namespace Assets.Scripts.Core.Interfaces.Items
{
    public interface IUsable
    {
        public void Use(IHealthSystem healthSystem);
    }
}