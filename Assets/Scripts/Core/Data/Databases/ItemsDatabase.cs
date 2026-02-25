using Assets.Scripts.Runtime.SOScripts;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Items Database")]
public class ItemsDatabase : ScriptableObject
{
    public List<InventoryItemSO> AllItems;

    private Dictionary<string, InventoryItemSO> _cache;

    public void Init()
    {
        _cache = new Dictionary<string, InventoryItemSO>();
        foreach (var item in AllItems)
        {
            _cache[item.ItemID] = item;
        }
    }

    public InventoryItemSO GetById(string id)
    {
        return _cache.TryGetValue(id, out var item) ? item : null;
    }
}
