using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.ParticleSystem;

public class Projectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] LayerMask _enemyLayers;
    [SerializeField] LayerMask _friendlyLayers;
    [SerializeField] private Rigidbody rb;

    private ProjectileSO _projectileSO;
    private Transform _target;
    private bool _isReleased = false;
    private UnitType _typeToDamage;


    private IObjectPool<Projectile> _pool;
    public void Init(ProjectileSO projectileSO, Vector3 pos, Vector3 dir, Transform target,UnitType typeToDamage,
        IObjectPool<Projectile> pool)
    {
        _projectileSO = projectileSO;
        _typeToDamage = typeToDamage;
        _target = target;
        _pool = pool;
        _isReleased = false;
        if (_trail != null)
        {
            _trail.enabled = false; 
            _trail.Clear();
        }

        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(dir);
        rb.position = pos;
        rb.rotation = Quaternion.LookRotation(dir);


        rb.linearVelocity = transform.forward * projectileSO.Speed;
        rb.angularVelocity = Vector3.zero;
        if (_trail != null)
        {
            _trail.transform.SetParent(this.transform);
            _trail.transform.position = this.transform.position;
            _trail.enabled = true;
        }
        AutoDespawn().Forget();
    }

    private void FixedUpdate()
    {
        if (_projectileSO.Homing && _target != null && _target == null)
        {
            Vector3 dir = (_target.position - rb.position);
            dir.y = 0; 
            dir = dir.normalized;
            Quaternion targetRot = Quaternion.LookRotation(dir);
            Quaternion nextRot = Quaternion.RotateTowards(
                rb.rotation,
                targetRot,
                _projectileSO.HomingRotationSpeed * Time.fixedDeltaTime
            );

            rb.MoveRotation(nextRot); 

            rb.linearVelocity = transform.forward * _projectileSO.Speed;
        }
        else
        {
            rb.linearVelocity = transform.forward * _projectileSO.Speed;
           
        }
    }
    private async UniTaskVoid AutoDespawn()
    {
        await UniTask.Delay((int)(_projectileSO.Lifetime * 1000f));

        if (!_isReleased)
            Despawn();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_isReleased)
            return;
        int hitLayer = collision.gameObject.layer;
        print(hitLayer);
        LayerMask targetLayers;
        switch (_typeToDamage)
        {
            case UnitType.Enemy:
                targetLayers = _enemyLayers;
                break;
            case UnitType.Friendly:
                targetLayers = _friendlyLayers;
                break;
            default:
                targetLayers = _enemyLayers;
                break;
        }

        if (((1 << hitLayer) & targetLayers) == 0)
        {
            DetachTrail();
            Despawn();
            return;
        }
        var hp = collision.collider.GetComponent<IHealthSystem>();
        hp?.TakeDamage(_projectileSO.Damage);
        DetachTrail();
        Despawn();
    }
    private void DetachTrail()
    {
        if (_trail == null) return;

        _trail.transform.SetParent(null);            
         
    }
    private void Despawn()
    {
        if (_isReleased)
        {
            return;
        }
            
        _isReleased = true;
        _pool.Release(this);
    }
}
