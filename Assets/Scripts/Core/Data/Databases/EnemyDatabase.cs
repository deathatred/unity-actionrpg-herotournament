using Assets.Scripts.Runtime.SOScripts.EnemiesSO;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/Enemy Database")]
public class EnemyDatabase : ScriptableObject
{
    public List<EnemyDataSO> AllEnemy;

    private Dictionary<string, EnemyDataSO> _cache;

    public void Init()
    {
        _cache = new Dictionary<string, EnemyDataSO>();
        foreach (var enemy in AllEnemy)
        {
            _cache[enemy.ID] = enemy;
        }
    }

    public EnemyDataSO GetById(string id)
    {
        return _cache.TryGetValue(id, out var enemy) ? enemy : null;
    }
}
