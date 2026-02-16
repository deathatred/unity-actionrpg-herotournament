using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StatsViewUI : MonoBehaviour
{
    [Inject] private EventBus _eventBus;

    [SerializeField] private TextMeshProUGUI _levelAmountText;
    [SerializeField] private TextMeshProUGUI _classNameText;
    [SerializeField] private Image _classImage;

    [SerializeField] private TextMeshProUGUI _hpAmountText;
    [SerializeField] private TextMeshProUGUI _mpAmountText;
    [SerializeField] private TextMeshProUGUI _strAmountText;
    [SerializeField] private TextMeshProUGUI _agiAmountText;
    [SerializeField] private TextMeshProUGUI _intAmountText;
    [SerializeField] private TextMeshProUGUI _atkPwrAmountText;
    [SerializeField] private TextMeshProUGUI _armorAmountText;
    [SerializeField] private TextMeshProUGUI _spellPwrAmountText;
    [SerializeField] private TextMeshProUGUI _moveSpdAmountText;
    [SerializeField] private TextMeshProUGUI _vampirismAmountText;
    [SerializeField] private TextMeshProUGUI _atkSpdAmountText;
    [SerializeField] private TextMeshProUGUI _critRateAmountText;

    [SerializeField] private Button _backButton;
    [SerializeField] private Button _addStrButton;
    [SerializeField] private Button _addAgiButton;
    [SerializeField] private Button _addIntButton;
    [SerializeField] private Button _talentTreeButton;
    [SerializeField] private Button _inventoryButton;
    private Dictionary<StatType, TextMeshProUGUI> _statsMap;

    private void Awake()
    {
        InitializeStatsMap();
        HideLevelUpButtons();
    }

    private void InitializeStatsMap()
    {
        _statsMap = new Dictionary<StatType, TextMeshProUGUI>
        {
            { StatType.MaxHealth, _hpAmountText },
            { StatType.MaxMana, _mpAmountText },
            { StatType.Strength, _strAmountText },
            { StatType.Agility, _agiAmountText },
            { StatType.Intellect, _intAmountText },
            { StatType.AttackPower, _atkPwrAmountText },
            { StatType.Armor, _armorAmountText },
            { StatType.SpellPower, _spellPwrAmountText },
            { StatType.MoveSpeed, _moveSpdAmountText },
            { StatType.Vampirism, _vampirismAmountText },
            { StatType.AttackSpeed, _atkSpdAmountText },
            { StatType.CriticalRate, _critRateAmountText }
        };
    }
    private void OnEnable()
    {
        BindButtons();
        SubscribeToEvents();
    }
    private void OnDisable()
    {
        UnbindButtons();
        UnsubscribeFromEvents();
    }
    private void BindButtons()
    {
        _backButton.onClick.AddListener(BackButtonPress);
        _inventoryButton.onClick.AddListener(InventoryButtonPress);
        _talentTreeButton.onClick.AddListener(TalentTreeButtonPress);
        _addStrButton.onClick.AddListener(AddStrPress);
        _addAgiButton.onClick.AddListener(AddAgiPress);
        _addIntButton.onClick.AddListener(AddIntPress);
    }
    private void UnbindButtons()
    {
        _backButton.onClick.RemoveListener(BackButtonPress);
        _inventoryButton.onClick.RemoveListener(InventoryButtonPress);
        _talentTreeButton.onClick.RemoveListener(TalentTreeButtonPress);
        _addStrButton.onClick.RemoveListener(AddStrPress);
        _addAgiButton.onClick.RemoveListener(AddAgiPress);
        _addIntButton.onClick.RemoveListener(AddIntPress);
    }
    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<PlayerConfiguredEvent>(OnPlayerConfigured);
        _eventBus.Subscribe<StatChangedEvent>(OnStatChanged);
        _eventBus.Subscribe<AllStatsChangedEvent>(AllStatsChanged);
        _eventBus.Subscribe<PlayerLevelChangedEvent>(PlayerLevelUp);
        _eventBus.Subscribe<PlayerLevelRestoredEvent>(PlayerLevelRestored);
        _eventBus.Subscribe<PlayerLevelPointsSpentEvent>(OnLevelPointChanged);
        _eventBus.Subscribe<PlayerSpecChosenEvent>(OnPlayerSpecChosen);
    }

    private void UnsubscribeFromEvents()
    {
        _eventBus.Unsubscribe<PlayerConfiguredEvent>(OnPlayerConfigured);
        _eventBus.Unsubscribe<StatChangedEvent>(OnStatChanged);
        _eventBus.Unsubscribe<AllStatsChangedEvent>(AllStatsChanged);
        _eventBus.Unsubscribe<PlayerLevelChangedEvent>(PlayerLevelUp);
        _eventBus.Unsubscribe<PlayerLevelRestoredEvent>(PlayerLevelRestored);
        _eventBus.Unsubscribe<PlayerLevelPointsSpentEvent>(OnLevelPointChanged);
        _eventBus.Unsubscribe<PlayerSpecChosenEvent>(OnPlayerSpecChosen);
    }
    private void BackButtonPress()
    {
        _eventBus.Publish(new BackButtonPressedEvent(BackButtonCaller.StatsMenu));
    }
    private void AllStatsChanged(AllStatsChangedEvent e)
    {
        foreach (var pair in e.Stats)
        {
            if (!_statsMap.TryGetValue(pair.Key, out var text))
                continue;

            bool isPercentage =
                pair.Key == StatType.SpellPower ||
                pair.Key == StatType.Vampirism ||
                pair.Key == StatType.CriticalRate;

            text.text = isPercentage
                ? $"{pair.Value}%"
                : pair.Value.ToString();
        }
    }
    private void InventoryButtonPress()
    {
        _eventBus.Publish(new InventoryPressedEvent());
    }
    private void TalentTreeButtonPress()
    {
        _eventBus.Publish(new TalentTreeButtonPressedEvent());
    }
    private void AddStrPress()
    {
        _eventBus.Publish(new AddStrButtonPressedEvent());
    }
    private void AddAgiPress()
    {
        _eventBus.Publish(new AddAgiButtonPressedEvent());
    }
    private void AddIntPress()
    {
        _eventBus.Publish(new AddIntButtonPressedEvent());
    }
    private void OnPlayerConfigured(PlayerConfiguredEvent e)
    {
        _classNameText.text = e.PlayerClassSO.ClassName.ToString();
        SetClassIcon(e.PlayerClassSO.ClassIcon);
    }
    private void OnPlayerSpecChosen(PlayerSpecChosenEvent e)
    {
        SetClassIcon(e.Spec.Icon);
    }
    private void SetClassIcon(Sprite icon)
    {
        _classImage.sprite = icon;
    }
    private void OnStatChanged(StatChangedEvent e)
    {
        if (_statsMap.TryGetValue(e.StatType, out var textMesh))
        {
            bool isPercentage = e.StatType == StatType.SpellPower || e.StatType == StatType.Vampirism || e.StatType == StatType.CriticalRate;
            textMesh.text = isPercentage ? $"{e.Amount}%" : e.Amount.ToString();
        }
    }

    private void PlayerLevelUp(PlayerLevelChangedEvent e)
    {
        _levelAmountText.text = e.Level.ToString();
        ShowLevelUpButtons(true);
    }

    private void PlayerLevelRestored(PlayerLevelRestoredEvent e)
    {
        _levelAmountText.text = e.RestoredLevel.ToString();
        ShowLevelUpButtons(e.RestoredLevelPoints > 0);
    }

    private void OnLevelPointChanged(PlayerLevelPointsSpentEvent e)
    {
        ShowLevelUpButtons(e.Amount > 0);
    }

    private void ShowLevelUpButtons(bool state)
    {
        _addStrButton.gameObject.SetActive(state);
        _addAgiButton.gameObject.SetActive(state);
        _addIntButton.gameObject.SetActive(state);
    }

    private void HideLevelUpButtons() => ShowLevelUpButtons(false);
}