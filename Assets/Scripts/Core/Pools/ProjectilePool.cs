using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class ProjectilePool : MonoBehaviour
{
    private Dictionary<ProjectileSO, IObjectPool<Projectile>> _pools =
        new Dictionary<ProjectileSO, IObjectPool<Projectile>>();

    public Projectile Get(ProjectileSO config)
    {
        if (!_pools.TryGetValue(config, out var pool))
        {
            pool = CreateNewPool(config);
            _pools.Add(config, pool);
        }

        return pool.Get();
    }

    private IObjectPool<Projectile> CreateNewPool(ProjectileSO config)
    {
        return new ObjectPool<Projectile>(
            () => Instantiate(config.Prefab).GetComponent<Projectile>(),
            p => p.gameObject.SetActive(true),
            p => p.gameObject.SetActive(false),
            p => Destroy(p.gameObject),
            maxSize: GlobalData.DEFAULT_PROJECTILE_POOL_SIZE  
        );
    }

    public void SpawnProjectile(ProjectileSO so, Vector3 pos, Vector3 dir,UnitType typeToDamage,int finalDamage, Transform target = null)
    {
        var projectile = Get(so);
        projectile.Init(so, pos, dir, target, typeToDamage, _pools[so], finalDamage);
    }
}
