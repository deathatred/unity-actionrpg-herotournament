using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.Events;
using Assets.Scripts.Runtime.Events.GameLevelEvents;
using Assets.Scripts.Runtime.Events.PlayerLevelSystemEvents;
using Assets.Scripts.Runtime.Events.PlayerSpellCastEvent;
using Assets.Scripts.Runtime.Events.StatsEvents.NewOnes;
using Assets.Scripts.Runtime.UI;
using Assets.Scripts.Runtime.UI.UIEvents;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Runtime.UI.Views
{
    public class GameViewUI : MonoBehaviour
    {
        [Inject] private EventBus _eventBus;

        [SerializeField] private Image _healthBarImage;
        [SerializeField] private Image _manaBarImage;
        [SerializeField] private Image _classIconImage;
        [SerializeField] private Image _xpIndicatorImage;
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private TextMeshProUGUI _manaText;
        [SerializeField] private TextMeshProUGUI _maxHealthText;
        [SerializeField] private TextMeshProUGUI _maxManaText;
        [SerializeField] private TextMeshProUGUI _levelText;

        [SerializeField] private SpellHotkeysUI _firstSpellHotkeys;
        [SerializeField] private Button _firstSpellButton;
        [SerializeField] private SpellHotkeysUI _secondSpellHotkeys;
        [SerializeField] private Button _secondSpellButton;
        [SerializeField] private SpellHotkeysUI _thirdSpellHotkeys;
        [SerializeField] private Button _thirdSpellButton;

        [SerializeField] private Image _firstItemImage;
        [SerializeField] private Image _secondItemImage;
        [SerializeField] private TextMeshProUGUI _firstItemHotkeyText;
        [SerializeField] private TextMeshProUGUI _secondItemHotkeyText;

        [SerializeField] private TextMeshProUGUI _enemiesAmountText;
        [SerializeField] private TextMeshProUGUI _timeAmountText;

        [SerializeField] private Button _menuButton;

        private CancellationTokenSource _hpCts = new CancellationTokenSource();
        private CancellationTokenSource _xpCts = new CancellationTokenSource();
        private CancellationTokenSource _mpCts = new CancellationTokenSource();
        private CancellationTokenSource _timerCts = new CancellationTokenSource();

        private void OnEnable()
        {
            BindButtons();
            SubscribeToEvents();
        }
        private void Awake()
        {
            _xpIndicatorImage.fillAmount = 0;
        }
        private void OnDisable()
        {
            UnbindButtons();
            UnsubscribeFromEvents();

            CancelToken(ref _hpCts);
            CancelToken(ref _xpCts);
            CancelToken(ref _mpCts);
            CancelToken(ref _timerCts);

            _healthBarImage?.DOKill();
            _manaBarImage?.DOKill();
            _xpIndicatorImage?.DOKill();
        }

        private void BindButtons()
        {
            _menuButton.onClick.AddListener(MenuButtonClicked);
            _firstSpellButton.onClick.AddListener(FirstSpellButtonClicked);
            _secondSpellButton.onClick.AddListener(SecondSpellButtonClicked);
            _thirdSpellButton.onClick.AddListener(ThirdSpellButtonClicked);
        }
        private void UnbindButtons()
        {
            _menuButton.onClick.RemoveListener(MenuButtonClicked);
            _firstSpellButton.onClick.RemoveListener(FirstSpellButtonClicked);
            _secondSpellButton.onClick.RemoveListener(SecondSpellButtonClicked);
            _thirdSpellButton.onClick.RemoveListener(ThirdSpellButtonClicked);
        }
        private void MenuButtonClicked()
        {
            _eventBus.Publish(new MenuButtonPressedEvent());
        }
        private void FirstSpellButtonClicked()
        {
            _eventBus.Publish(new FirstSpellButtonPressedEvent());
        }
        private void SecondSpellButtonClicked()
        {
            _eventBus.Publish(new SecondSpellButtonPressedEvent());
        }
        private void ThirdSpellButtonClicked()
        {
            _eventBus.Publish(new ThirdSpellButtonPressedEvent());
        }
        private void SubscribeToEvents()
        {
            _eventBus.Subscribe<StatChangedEvent>(StatChanged);
            _eventBus.Subscribe<CurrentHealthChangedEvent>(ChangeCurrentHpText);
            _eventBus.Subscribe<CurrentManaChangedEvent>(ChangeCurrentManaText);
            _eventBus.Subscribe<PlayerHealthChangedEvent>(ChangeHealth);
            _eventBus.Subscribe<PlayerGainedXpEvent>(ChangeXp);
            _eventBus.Subscribe<PlayerLevelChangedEvent>(LevelUp);
            _eventBus.Subscribe<PlayerLevelRestoredEvent>(RestoreLevel);
            _eventBus.Subscribe<PlayerManaChangedEvent>(ChangeMana);
            _eventBus.Subscribe<PlayerSpellUnlockedEvent>(ShowLearnedSpell);
            _eventBus.Subscribe<PlayerSpellCastedEvent>(ShowCooldown);
            _eventBus.Subscribe<AmountOfMobsOnLevelDecreasedEvent>(ReduceEnemiesLeftText);
            _eventBus.Subscribe<SurvivalModeStartedEvent>(StartTimerSubscriber);
            _eventBus.Subscribe<DefaultModeStartedEvent>(EnableEnemyAmountSubscriber);
            _eventBus.Subscribe<PlayerConfiguredEvent>(SetClassIcon);
            _eventBus.Subscribe<PlayerSpecChosenEvent>(SetSpecIcon);
        }
        private void UnsubscribeFromEvents()
        {
            _eventBus.Unsubscribe<StatChangedEvent>(StatChanged);
            _eventBus.Unsubscribe<CurrentHealthChangedEvent>(ChangeCurrentHpText);
            _eventBus.Unsubscribe<CurrentManaChangedEvent>(ChangeCurrentManaText);
            _eventBus.Unsubscribe<PlayerHealthChangedEvent>(ChangeHealth);
            _eventBus.Unsubscribe<PlayerGainedXpEvent>(ChangeXp);
            _eventBus.Unsubscribe<PlayerLevelChangedEvent>(LevelUp);
            _eventBus.Unsubscribe<PlayerLevelRestoredEvent>(RestoreLevel);
            _eventBus.Unsubscribe<PlayerManaChangedEvent>(ChangeMana);
            _eventBus.Unsubscribe<PlayerSpellUnlockedEvent>(ShowLearnedSpell);
            _eventBus.Unsubscribe<PlayerSpellCastedEvent>(ShowCooldown);
            _eventBus.Unsubscribe<AmountOfMobsOnLevelDecreasedEvent>(ReduceEnemiesLeftText);
            _eventBus.Unsubscribe<SurvivalModeStartedEvent>(StartTimerSubscriber);
            _eventBus.Unsubscribe<DefaultModeStartedEvent>(EnableEnemyAmountSubscriber);
            _eventBus.Unsubscribe<PlayerConfiguredEvent>(SetClassIcon);
            _eventBus.Unsubscribe<PlayerSpecChosenEvent>(SetSpecIcon);
        }
        private void SetClassIcon(PlayerConfiguredEvent e)
        {
            ChangeClassIcon(e.PlayerClassSO.ClassIcon);
            _classIconImage.sprite = e.PlayerClassSO.ClassIcon;
        }
        private void SetSpecIcon(PlayerSpecChosenEvent e)
        {
            ChangeClassIcon(e.Spec.Icon);
        }
        private void ChangeClassIcon(Sprite icon)
        {
            _classIconImage.sprite = icon;
        }
        private void StatChanged(StatChangedEvent e)
        {
            if (e.StatType == StatType.MaxHealth)
            {
                _maxHealthText.text = $"/{e.Amount.ToString()}";
                return;
            }
            else if (e.StatType == StatType.MaxMana)
            {
                _maxManaText.text = $"/{e.Amount.ToString()}";
                return;
            }
        }
        private void ChangeHealth(PlayerHealthChangedEvent e)
        {
            CancelToken(ref _hpCts);
            AnimateFillAsync(_healthBarImage, null, e.CurrentHealth, e.MaxHealth, 0.3f, _hpCts).Forget();
        }
        private void ChangeXp(PlayerGainedXpEvent e)
        {
            CancelToken(ref _xpCts);
            AnimateFillAsync(_xpIndicatorImage, null, e.CurrentXp, e.MaxXp, 0.3f, _xpCts).Forget();
        }
        private void ChangeMana(PlayerManaChangedEvent e)
        {
            CancelToken(ref _mpCts);
            AnimateFillAsync(_manaBarImage, _manaText, e.CurrentMana, e.MaxMana, 0.3f, _mpCts).Forget();
        }
        private void LevelUp(PlayerLevelChangedEvent e)
        {
            FillXpAsync().Forget();
            _levelText.text = e.Level.ToString();
        }
        private void RestoreLevel(PlayerLevelRestoredEvent e)
        {
            _levelText.text = e.RestoredLevel.ToString();
            AnimateFillAsync(_xpIndicatorImage, null, e.CurrentXp, e.XpToNextLevel, 0.1f, _xpCts).Forget();
        }
        private void ChangeCurrentHpText(CurrentHealthChangedEvent e)
        {
            _healthText.text = e.Amount.ToString();
        }
        private void ChangeCurrentManaText(CurrentManaChangedEvent e)
        {
            _manaText.text = e.Amount.ToString();
        }
        private void ReduceEnemiesLeftText(AmountOfMobsOnLevelDecreasedEvent e)
        {
            if (e.AmountOfMobs <= 0)
            {
                _enemiesAmountText.text = $"Portal Opened";
                _enemiesAmountText.color = Color.magenta;
                return;
            }
            _enemiesAmountText.text = $"Enemies left:{e.AmountOfMobs}";
        }
        private void ShowLearnedSpell(PlayerSpellUnlockedEvent e)
        {
            if (!_firstSpellHotkeys.IsSet)
            {
                _firstSpellHotkeys.SetSpell(e.SpellSO);
                return;
            }
            if (!_secondSpellHotkeys.IsSet)
            {
                _secondSpellHotkeys.SetSpell(e.SpellSO);
                return;
            }
            else
            {
                _thirdSpellHotkeys.SetSpell(e.SpellSO);
                return;
            }
        }
        private void ShowCooldown(PlayerSpellCastedEvent e)
        {
            _firstSpellHotkeys.TryStartCooldown(e.Spell);
            _secondSpellHotkeys.TryStartCooldown(e.Spell);
            _thirdSpellHotkeys.TryStartCooldown(e.Spell);
        }
        private void StartTimerSubscriber(SurvivalModeStartedEvent e)
        {
            CancelToken(ref _timerCts);
            EnableTimer();
            StartTimer(e.Time, _timerCts).Forget();
        }
        private void CancelToken(ref CancellationTokenSource token)
        {
            if (token == null)
                return;

            if (!token.IsCancellationRequested)
                token.Cancel();

            token.Dispose();
            token = new CancellationTokenSource();
        }
        private void EnableEnemyAmount()
        {
            _enemiesAmountText.gameObject.SetActive(true);
            _timeAmountText.gameObject.SetActive(false);
        }
        private void EnableEnemyAmountSubscriber(DefaultModeStartedEvent e)
        {
            _enemiesAmountText.gameObject.SetActive(true);
            _timeAmountText.gameObject.SetActive(false);
            _enemiesAmountText.color = Color.red;
            _enemiesAmountText.text = $"Enemies left:{e.AmountOfEnemies}";
        }
        private void EnableTimer()
        {
            _timeAmountText.gameObject.SetActive(true);
            _enemiesAmountText.gameObject.SetActive(false);
        }
        public void FillXpIndicatorInstant(float value)
        {
            _xpIndicatorImage.fillAmount = value;
        }
        private async UniTask AnimateFillAsync(Image fillImage, TextMeshProUGUI optionalText, int currentValue,
         int maxValue, float duration, CancellationTokenSource cts)
        {

            if (optionalText != null)
                optionalText.text = currentValue.ToString();

            float targetFill = maxValue > 0 ? (float)currentValue / maxValue : 0f;
            fillImage.DOKill();
            var tweener = fillImage
                .DOFillAmount(targetFill, duration)
                .SetEase(Ease.Linear);

            try
            {
                await tweener.AsyncWaitForCompletion().AsUniTask()
                    .AttachExternalCancellation(cts.Token);
            }
            catch (OperationCanceledException)
            {
            }
        }
        private async UniTask FillXpAsync()
        {
            try
            {
                CancelToken(ref _xpCts);
                await _xpIndicatorImage.DOFillAmount(1, 0.3f).SetEase(Ease.Linear)
                    .AsyncWaitForCompletion().AsUniTask().AttachExternalCancellation(_xpCts.Token);
                _xpIndicatorImage.fillAmount = 0;
            }
            catch (OperationCanceledException)
            {

            }
        }
        private async UniTask StartTimer(int time, CancellationTokenSource cts)
        {
            try
            {


                int maxTime = time;
                while (maxTime != 0 && !cts.IsCancellationRequested)
                {
                    _timeAmountText.text = $"time left: {maxTime}";
                    maxTime--;
                    await UniTask.WaitForSeconds(1f, cancellationToken: cts.Token);
                }
                if (cts.IsCancellationRequested) return;
                EnableEnemyAmount();
                _enemiesAmountText.text = $"Portal Opened";
                _enemiesAmountText.color = Color.magenta;
            }
            catch (OperationCanceledException)
            {

            }
        }
    }
}