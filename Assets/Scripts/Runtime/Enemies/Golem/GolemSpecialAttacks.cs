using Assets.Scripts.Core.Interfaces;
using Unity.Cinemachine;
using UnityEngine;

public class GolemSpecialAttacks : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayerMask;
    [SerializeField] private LayerMask _obstacleLayerMask; 
    [SerializeField] private Transform _smashOrigin;
    [SerializeField] private ParticleSystem _dirtParticles;

    private float _smashRadius = 13f;
    private int _smashDamage = 30;
    public void TriggerSmash()
    {
        PlayParticles();
        Vector3 origin = _smashOrigin != null
            ? _smashOrigin.position
            : transform.position;

        Collider[] hits = Physics.OverlapSphere(
            origin,
            _smashRadius,
            _playerLayerMask
        );

        foreach (var hit in hits)
        {
            Vector3 targetPoint = hit.bounds.center;
            Vector3 dir = targetPoint - origin;
            float dist = dir.magnitude;
            dir.Normalize();

            if (Physics.Raycast(origin,dir,out RaycastHit rayHit,dist,_obstacleLayerMask | _playerLayerMask))
            {
                if (((1 << rayHit.collider.gameObject.layer) & _playerLayerMask) != 0)
                {
                    if (hit.TryGetComponent<IHealthSystem>(out var damageable))
                    {
                        damageable.TakeDamage(_smashDamage);
                    }
                }
            }
        }
    }
    private void PlayParticles()
    {
        ParticleSystem particle = Instantiate(_dirtParticles, _smashOrigin);
        particle.transform.rotation = Quaternion.Euler(-90, 0, 0);
        particle.Play();
    }
    private void OnDrawGizmosSelected()
    {
        Vector3 origin = _smashOrigin != null
            ? _smashOrigin.position
            : transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, _smashRadius);
    }
}

