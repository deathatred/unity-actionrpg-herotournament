using UnityEngine;
using Zenject;

public class EnemyProjectileShootingBase : MonoBehaviour
{
    [Inject] private ProjectilePool _projectilePool;
    [SerializeField] private ProjectileSO _projectileSO;
    [SerializeField] private EnemyTargetDetector _enemyTargetDetector;
    [SerializeField] private Transform _shootPoint;
    
    public void ShootProjectile(Transform target = null)
    {
        Vector3 pos = _shootPoint.position;
        Vector3 dir = _shootPoint.forward;

        _projectilePool.SpawnProjectile(_projectileSO, pos, dir, UnitType.Friendly,_projectileSO.Damage, target);
    }

}
