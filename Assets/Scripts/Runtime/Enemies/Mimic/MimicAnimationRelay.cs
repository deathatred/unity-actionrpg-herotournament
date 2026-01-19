using UnityEngine;
using Zenject;

public class MimicAnimationRelay : MonoBehaviour
{
    [SerializeField] private EnemyTargetDetector _detector;
    [SerializeField] private MimicAudio _audio;
    [SerializeField] private EnemyData _mimicData;

    private EnemyDataSO _mimicDataSO;
    private float _sfxVolume = 0.1f;
    private void Awake()
    {
        _mimicDataSO = _mimicData.GetEnemyData();
    }
    public void DealDamage()
    {
        var target = _detector.GetCurrentTarget();
        target.HealthSystem.TakeDamage(_mimicDataSO.Damage);
        _audio.PlayAttack(_sfxVolume);
    }
    public void PlayMoveSound()
    {
        _audio.PlayMove(_sfxVolume);
    }
    public void PlayDeath()
    {
        _audio.PlayDeath(_sfxVolume);
    }
}
