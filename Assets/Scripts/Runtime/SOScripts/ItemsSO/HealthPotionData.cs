using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Interfaces.Items;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Health Potion")]
public class HealthPotionData : InventoryItemSO, IUsable
{
    [SerializeField] private int _healAmount;
    public void Use(IHealthSystem healthSystem)
    {
        healthSystem.Heal(_healAmount);
    }
}
