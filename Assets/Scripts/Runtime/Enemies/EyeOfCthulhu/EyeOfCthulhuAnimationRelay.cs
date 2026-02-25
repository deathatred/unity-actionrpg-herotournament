using Assets.Scripts.Runtime.Enemies.EnemyBase;
using UnityEngine;

namespace Assets.Scripts.Runtime.Enemies.EyeOfCthulhu
{
    public class EyeOfCthulhuAnimationRelay : MonoBehaviour
    {
        [SerializeField] private EyeOfCthulhuAudio _audio;
        [SerializeField] private EnemyProjectileShootingBase _shooting;
        [SerializeField] private EnemyTargetDetector _detector;

        private float _sfxVolume = 0.1f;
        public void Shoot()
        {
            _audio.PlayAttack(_sfxVolume);
            _shooting.ShootProjectile(_detector.transform);

        }
        public void PlayDeath()
        {
            _audio.PlayDeath(_sfxVolume);
        }
    }
}