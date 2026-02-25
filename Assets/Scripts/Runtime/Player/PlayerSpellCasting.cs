using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.General;
using Assets.Scripts.Core.Observer;
using Assets.Scripts.Core.Pools;
using Assets.Scripts.Runtime.Enemies.EnemyBase;
using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Events;
using Assets.Scripts.Runtime.Events.PlayerSpellCastEvent;
using Assets.Scripts.Runtime.Events.StatsEvents.NewOnes;
using Assets.Scripts.Runtime.Player.FSM;
using Assets.Scripts.Runtime.Player.UniqueClassLogic;
using Assets.Scripts.Runtime.SOScripts;
using Assets.Scripts.Runtime.SOScripts.SpellSOs;
using Assets.Scripts.Runtime.UI.UIEvents;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Runtime.Player
{
    [DefaultExecutionOrder(-1)]
    public class PlayerSpellCasting : MonoBehaviour
    {
        [Inject] private DiContainer _container;
        [Inject] private EventBus _eventBus;
        [Inject] private PlayerAudio _playerAudio;
        [Inject] private PlayerStats _playerStats;
        [Inject] private PlayerAttackSystem _attackSystem;
        [Inject] private PlayerAnimations _animation;
        [Inject] private PlayerInputHandler _input;
        [Inject] private PlayerSpellbook _spellBook;
        [Inject] private PlayerStateMachine _fsm;
        [Inject] private PlayerInteractions _interaction;
        [Inject] private PlayerController _controller;
        [Inject] private ProjectilePool _projectilePool;

        public int MaxMana { get; private set; }
        public int CurrentMana { get; private set; }
        public SpellSO LastCastSpell { get; private set; }

        private PlayerClass _class;
        private SpellSO _firstSpell;
        private SpellSO _secondSpell;
        private SpellSO _thirdSpell;
        private SpellSO _queuedSpell;
        private CancellationTokenSource _cts;
        private bool _isCasted;
        private EnemyHealthSystem _castTarget;


        private Dictionary<SpellSO, float> _cooldowns = new();

        private void OnEnable()
        {
            _cts = new CancellationTokenSource();
            SubscribeToEvents();
        }
        private void Update()
        {
            HandleSpellCasting();

        }
        private void OnDisable()
        {
            _cts.Cancel();
            _cts.Dispose();
            UnsubscribeFromEvents();
        }
        private void SubscribeToEvents()
        {
            _eventBus.Subscribe<StatChangedEvent>(MaxManaChanged);
            _eventBus.Subscribe<PlayerSpellUnlockedEvent>(EquipSpell);
            _eventBus.Subscribe<FirstSpellButtonPressedEvent>(CastFirstSpell);
            _eventBus.Subscribe<SecondSpellButtonPressedEvent>(CastSecondSpell);
            _eventBus.Subscribe<ThirdSpellButtonPressedEvent>(CastThirdSpell);
            _eventBus.Subscribe<ReachedSpellTarget>(SpellTargetReached);
        }
        private void UnsubscribeFromEvents()
        {
            _eventBus.Unsubscribe<StatChangedEvent>(MaxManaChanged);
            _eventBus.Unsubscribe<PlayerSpellUnlockedEvent>(EquipSpell);
            _eventBus.Unsubscribe<FirstSpellButtonPressedEvent>(CastFirstSpell);
            _eventBus.Unsubscribe<SecondSpellButtonPressedEvent>(CastSecondSpell);
            _eventBus.Unsubscribe<ThirdSpellButtonPressedEvent>(CastThirdSpell);
            _eventBus.Unsubscribe<ReachedSpellTarget>(SpellTargetReached);
        }
        private void HandleSpellCasting()
        {
            if (!_input.FirstSpellCast &&
           !_input.SecondSpellCast &&
           !_input.ThirdSpellCast)
            {
                _isCasted = false;
            }
            CastSpellWithInput(_input.FirstSpellCast, _firstSpell);
            CastSpellWithInput(_input.SecondSpellCast, _secondSpell);
            CastSpellWithInput(_input.ThirdSpellCast, _thirdSpell);
        }
        private void CastSpellWithInput(bool input, SpellSO spell)
        {
            if (input)
            {
                TryCastSpellAsync(spell, _cts).Forget();
            }
        }
        private void CastFirstSpell(FirstSpellButtonPressedEvent e)
        {
            TryCastSpellAsync(_firstSpell, _cts).Forget();
        }
        private void CastSecondSpell(SecondSpellButtonPressedEvent e)
        {
            TryCastSpellAsync(_secondSpell, _cts).Forget();
        }
        private void CastThirdSpell(ThirdSpellButtonPressedEvent e)
        {
            TryCastSpellAsync(_thirdSpell, _cts).Forget();
        }
        private void SpellTargetReached(ReachedSpellTarget e)
        {
            TryCastSpellAsync(_queuedSpell, _cts).Forget();
        }
        private bool IsOnCooldown(SpellSO spell)
        {
            if (!_cooldowns.TryGetValue(spell, out float nextAvailableTime))
                return false;

            return Time.time < nextAvailableTime;
        }
        private void MaxManaChanged(StatChangedEvent e)
        {
            if (e.StatType == StatType.MaxMana)
            {
                float oldMax = MaxMana;
                float oldCurrent = CurrentMana;

                MaxMana = e.Amount;
                if (oldMax > 0f)
                {
                    CurrentMana = (int)((float)oldCurrent / oldMax * MaxMana);
                }
                else
                {
                    CurrentMana = MaxMana;
                }
                _eventBus.Publish(new CurrentManaChangedEvent(CurrentMana));
            }
        }
        private void EquipSpell(PlayerSpellUnlockedEvent e)
        {
            if (_firstSpell == null)
            {
                _firstSpell = e.SpellSO;
                return;
            }
            if (_secondSpell == null)
            {
                _secondSpell = e.SpellSO;
            }
            else
            {
                _thirdSpell = e.SpellSO;
            }
        }
        public bool IsInSpellRange(SpellSO spell)
        {
            if (spell.Range <= 0)
                return true;

            var target = _interaction.GetCurrentEnemyTarget();
            if (target == null)
                return false;
            Vector3 dir = target.transform.position - _controller.GetRb().position;
            dir.y = 0;
            return dir.magnitude <= spell.Range;
        }
        public PlayerAttackSystem GetAttackSystem()
        {
            return _attackSystem;
        }
        public void CastSpell(SpellSO spellSO)
        {
            if (_fsm.CurrentState == PlayerState.SpellCasting)
            {
                return;
            }
            CurrentMana -= spellSO.ManaCost;
            LastCastSpell = spellSO;
            _animation.SetSpellTrigger(spellSO.AnimationTrigger);
            _cooldowns[spellSO] = Time.time + spellSO.Cooldown;
            _eventBus.Publish(new PlayerSpellCastedEvent(spellSO));
            _eventBus.Publish(new PlayerManaChangedEvent(CurrentMana, MaxMana));
        }
        public void SpellCastingEnded()
        {
            _eventBus.Publish(new PlayerSpellCastEnded());
        }
        public void ExecuteSummoningSpell(GameObject summonPrefab)
        {
            float radius = GlobalData.SUMMON_RADIUS;
            float angle = Random.Range(0f, 360f);

            Vector3 offset = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                0,
                Mathf.Sin(angle * Mathf.Deg2Rad)
            ) * radius;

            Vector3 summonPosition = transform.position + offset;

            _container.InstantiatePrefab(
                summonPrefab,
                summonPosition,
                Quaternion.identity,
                null
            );
        }
        public SpellSO GetQueuedSpell()
        {
            return _queuedSpell;
        }
        public void RestorePlayerMana(int amount)
        {
            MaxMana = _playerStats.GetBaseStat(StatType.MaxMana);
            CurrentMana = amount;
            _eventBus.Publish(new CurrentManaChangedEvent(CurrentMana));
            _eventBus.Publish(new PlayerManaChangedEvent(CurrentMana, MaxMana));
        }
        private async UniTask TryCastSpellAsync(SpellSO spell, CancellationTokenSource _cts)
        {
            if (_isCasted)
            {
                return;
            }
            if (spell == null)
            {
                return;
            }
            if (IsOnCooldown(spell))
            {
                return;
            }
            if (CurrentMana < spell.ManaCost)
            {
                return;
            }

            if (!IsInSpellRange(spell))
            {
                _queuedSpell = spell;
                _fsm.ChangeState(PlayerState.MoveToSpellTarget);
                return;
            }

            var target = _interaction.GetCurrentEnemyTarget();
            _isCasted = true;
            _castTarget = target;
            if (target != null)
            {
                await _controller.RotateToTargetAsync(target.transform.position, _cts.Token);
            }
            CastSpell(spell);
            _queuedSpell = null;

        }
        public EnemyHealthSystem GetCastTarget()
        {
            return _castTarget;
        }
        public async UniTask ExecuteProjectileSpellAsync(ProjectileSO projectileSO, Transform target = null)
        {
            await _controller.RotateToTargetAsync(target.transform.position, _cts.Token);
            var relay = GetComponentInChildren<PlayerAnimationRelayBase>();
            Vector3 pos = relay.GetShootPoint().position;
            Vector3 dirBefore = (target.position - pos).normalized;
            Vector3 dirAfter = new Vector3(dirBefore.x, transform.position.y, dirBefore.z);
            float spellPowerPercent = _playerStats.GetFinalStat(StatType.SpellPower) / 100f;
            int finalDamage = Mathf.RoundToInt(projectileSO.Damage * (1f + spellPowerPercent));
            _projectilePool.SpawnProjectile(projectileSO, pos, dirAfter, UnitType.Enemy, finalDamage);

        }

    }
}