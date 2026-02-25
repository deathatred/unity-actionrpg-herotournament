using Assets.Scripts.Runtime.Enemies.EnemyBase;
using Assets.Scripts.Runtime.SpellsContext;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Runtime.Player.UniqueClassLogic
{
    public abstract class PlayerAnimationRelayBase : MonoBehaviour
    {
        protected PlayerAudio _playerAudio;
        protected PlayerSpellCasting _playerSpell;
        protected PlayerAnimations _playerAnimations;
        protected PlayerAttackSystem _playerAttackSystem;
        protected PlayerInteractions _playerInteractions;
        protected PlayerHealthSystem _playerHealthSystem;
        protected PlayerStats _playerStats;
        [SerializeField] private Transform _shootPoint;

        [Inject]
        protected void Construct(
            PlayerAudio playerAudio,
            PlayerSpellCasting playerSpell,
            PlayerAnimations playerAnimations,
            PlayerAttackSystem playerAttackSystem,
            PlayerInteractions playerInteractions,
            PlayerHealthSystem playerHealthSystem,
            PlayerStats playerStats)
        {
            _playerAudio = playerAudio;
            _playerSpell = playerSpell;
            _playerAnimations = playerAnimations;
            _playerAttackSystem = playerAttackSystem;
            _playerInteractions = playerInteractions;
            _playerHealthSystem = playerHealthSystem;
            _playerStats = playerStats;
        }

        public void EndAttack()
        {
            _playerAnimations.EndAttackingAnimation();
            _playerAudio.PlayAttackAudio();
        }

        public void EndCollect()
        {
            _playerInteractions.GetCurrentItemTarget()?.Collect();
            _playerAnimations.SetIsCollectingFalse();
        }

        public void AttackEnded()
        {
            _playerAttackSystem.OnAttackAnimationFinished();
        }

        public void SpellEnded()
        {
            _playerSpell.SpellCastingEnded();
        }

        public void OnAnimationEvent()
        {
            var lastSpell = _playerSpell.LastCastSpell;

            if (lastSpell == null)
            {
                Debug.LogWarning("Last spell is null!");
                return;
            }

            var target = _playerSpell.GetCastTarget();

            EnemyStatusEffectsManager enemyStatusEffectManager = null;

            if (target != null)
            {
                if (!target.TryGetComponent(out enemyStatusEffectManager))
                {
                    Debug.LogWarning("Target has no EnemyStatusEffectsManager!");
                }
            }

            var ctx = new PlayerSpellContext(
                _playerHealthSystem,
                target?.transform,
                target,
                _playerStats,
                _playerSpell,
                _playerAudio,
                enemyStatusEffectManager
            );

            lastSpell.Activate(ctx);
        }



        public void PlayPreattackSound()
        {
            _playerAudio.PlayPreattackAudio();
        }

        public void PlayFootstepSound()
        {
            _playerAudio.PlayFootstepSound();
        }
        public Transform GetShootPoint()
        {
            return _shootPoint;
        }
    }
}