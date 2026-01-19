using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ItemSpawnManager 
{
    [Inject] private DiContainer _container;
    [Inject] private ItemsDatabase _database;
    public void SpawnItems(List<ItemSpawnData> spawnData, Transform levelTransform)
    {
        for (int i = 0; i < spawnData.Count; i++)
        {
            var data = spawnData[i];

            InventoryItemSO itemSo = _database.GetById(data.Item.ItemID);

            if (itemSo == null)
            {
                Debug.LogWarning($"Item with id {data.Item.ItemID} not found in database");
                continue;
            }

            GameObject go = _container.InstantiatePrefab(itemSo.Prefab,data.SpawnPoint.position,Quaternion.identity, levelTransform);
            InWorldItem item = go.GetComponentInChildren<InWorldItem>();
            item.SetSceneID(data.SpawnIndex);
        }
    }
}
