using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.General;
using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.Enemies.EnemyBase;
using Assets.Scripts.Runtime.Events;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Runtime.Player
{
    public class PlayerAttackSystem : MonoBehaviour
    {
        [Inject] private EventBus _eventBus;
        [Inject] private PlayerStats _playerStats;
        [Inject] private PlayerAnimations _playerAnimations;
        [Inject] private PlayerHealthSystem _playerHealthSystem;

        private float _attackRange;
        private Animator _animator;
        private CancellationTokenSource _cts = new();
        private EnemyHealthSystem _currentTarget;
        private bool _isAttacking;
        private void OnEnable()
        {
            SubscribeToEvents();
        }
        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }
        private void GetAnimator()
        {
            _animator = _playerAnimations.GetPlayerAnimator();
        }
        private void SubscribeToEvents()
        {
            _eventBus.Subscribe<PlayerAttackEndedEvent>(DealDamageSubscriber);
            _eventBus.Subscribe<PlayerConfiguredEvent>(SetAttackRange);
        }
        private void UnsubscribeFromEvents()
        {
            _eventBus.Unsubscribe<PlayerAttackEndedEvent>(DealDamageSubscriber);
            _eventBus.Unsubscribe<PlayerConfiguredEvent>(SetAttackRange);
        }
        private void DealDamageSubscriber(PlayerAttackEndedEvent e)
        {
            DealDamage();
        }
        private void SetAttackRange(PlayerConfiguredEvent e)
        {
            var classSO = e.PlayerClassSO;
            switch (classSO.ClassName)
            {
                case PlayerClass.Knight:
                    _attackRange = GlobalData.KNIGHT_ATTACK_RANGE;
                    break;
                case PlayerClass.Wizard:
                    _attackRange = GlobalData.MAGE_ATTACK_RANGE;
                    break;
            }
        }
        public void DealDamage(float multiplier = 1)
        {
            if (_currentTarget != null && !_currentTarget.IsDead)
            {
                int attackDamage = _playerStats.GetBaseStat(StatType.AttackPower);
                if (UnityEngine.Random.Range(0, 101) < _playerStats.GetBaseStat(StatType.CriticalRate))
                {
                    attackDamage = Mathf.RoundToInt(_playerStats.GetBaseStat(StatType.AttackPower) * multiplier * 1.5f);
                }
                int damageTakenByEnemy = _currentTarget.TakeDamage(attackDamage);
                if (_playerStats.GetBaseStat(StatType.Vampirism) > 0)
                {
                    float healAmount = damageTakenByEnemy * (_playerStats.GetBaseStat(StatType.Vampirism) / 100f);
                    _playerHealthSystem.Heal((int)healAmount);
                }
            }
        }
        public void OnAttackAnimationFinished()
        {
            _isAttacking = false;
        }

        public void CancelAttacking()
        {
            _cts.Cancel();
            _isAttacking = false;
        }
        public float GetAttackRange()
        {
            return _attackRange;
        }
        public async UniTask MeleeAttackAsync(EnemyHealthSystem enemy)
        {
            if (_isAttacking || enemy == null)
            {
                return;
            }
            if (enemy.IsDead) return;
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            _currentTarget = enemy;
            _isAttacking = true;
            try
            {
                while (!enemy.IsDead && !_cts.Token.IsCancellationRequested)
                {
                    float speed = _playerStats.GetBaseStat(StatType.AttackSpeed) / 100f;
                    if (_animator == null)
                    {
                        GetAnimator();
                        _animator.SetFloat("AttackSpeedMultiplier", speed);
                    }
                    else
                    {
                        _animator.SetFloat("AttackSpeedMultiplier", speed);
                    }
                    _eventBus.Publish(new PlayerStartedAttackEvent());
                    await UniTask.WaitUntil(() => !_isAttacking, cancellationToken: _cts.Token);
                    _isAttacking = true;
                }
            }
            catch (OperationCanceledException) { }
            finally
            {
                _isAttacking = false;
            }
        }
    }
}