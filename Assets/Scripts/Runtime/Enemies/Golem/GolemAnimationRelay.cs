using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class GolemAnimationRelay : MonoBehaviour
{
    [SerializeField] private GolemAudio _audio;
    [SerializeField] private EnemyTargetDetector _detection;
    [SerializeField] private GolemSpecialAttacks _golemSpecialAttacks;
    [SerializeField] private CinemachineImpulseSource _impulse;
    [SerializeField] private EnemyData _data;

    private float _sfxVolume = 0.1f;
    public void DealDamage()
    {
        var damageAmount = _data.GetEnemyData().Damage;
        var target = _detection.GetCurrentTarget();
        target.HealthSystem.TakeDamage(damageAmount);
        _audio.PlayAttack(_sfxVolume*2f);
    }
    public void SmashEnded()
    {
        _golemSpecialAttacks.TriggerSmash();
        _impulse.GenerateImpulse();
        _audio.PlaySmashSound();
    }
    public void OnMove()
    {
        _audio.PlayMove(_sfxVolume);
    }
    public void OnPreattack()
    {
        _audio.PlayPreattack(_sfxVolume);
    }
    public void OnDeath()
    {
        _audio.PlayDeath(_sfxVolume);
    }
}
