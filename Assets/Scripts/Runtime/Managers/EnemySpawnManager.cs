using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Runtime.SOScripts.EnemiesSO;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Runtime.Managers
{
    public class EnemySpawnManager
    {
        [Inject] private EnemyDatabase _enemyDatabase;
        [Inject] private DiContainer _container;
        public GameObject InitEnemy(Vector3 spawnPoint, EnemyDataSO enemyData)
        {
            GameObject go = _container.InstantiatePrefab(enemyData.Prefab, spawnPoint, Quaternion.identity, null);
            return go;

        }
        public GameObject RestoreEnemy(EnemySaveData data)
        {
            var enemy = _enemyDatabase.GetById(data.EnemyId).Prefab;
            GameObject go = _container.InstantiatePrefab(enemy, data.Position, Quaternion.identity, null);
            go.GetComponent<IHealthSystem>().RestoreHealth(data.CurrentHealth);
            return go;
        }
    }
}