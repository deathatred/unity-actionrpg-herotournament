using Assets.Scripts.Core.Interfaces;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Runtime.Summons.Skeleton
{
    public class SkeletonAnimationRelay : MonoBehaviour
    {
        [SerializeField] private SummonData _data;
        [SerializeField] private SkeletonAudio _audio;
        [SerializeField] private SkeletonController _controller;
        private int _damage;
        private float _sfxVolume = 0.1f;

        private void Awake()
        {
            _damage = _data.GetSummonData().Damage;
        }
        public void EndAttack()
        {
            IHealthSystem health = _controller.AttackTarget.GetComponent<IHealthSystem>();
            health.TakeDamage(_damage);
            _audio.PlayAttack(_sfxVolume);
        }
        public void PlayPreattack()
        {
            _audio.PlayPreattack(_sfxVolume);
        }
        public void PlayDeath()
        {
            _audio.PlayDeath(_sfxVolume);
        }
        public void PlayMove()
        {
            _audio.PlayMove(0.01f);
        }

    }
}