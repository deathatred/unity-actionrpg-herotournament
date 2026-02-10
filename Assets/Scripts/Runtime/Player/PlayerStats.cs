using UnityEngine;
using Zenject;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class PlayerStats : MonoBehaviour
{
    [Inject] private EventBus _eventBus;
    private PlayerClassDefaultStatsSO _defaultStats;
    private Dictionary<StatType, int> _baseStats = new Dictionary<StatType, int>();
    private Dictionary<StatType, int> _outsideStats = new Dictionary<StatType, int>();
    private List<BonusEffect> _tempBonusEffects = new List<BonusEffect>();

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void Awake()
    {
        InitializeDictionary();
        InitializeOutsideDictionary();
    }
    public void Update()
    {
        foreach (var effect in _tempBonusEffects)
        {
            print(effect.StatType);
        }
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void InitializeDictionary()
    {
        foreach (StatType type in System.Enum.GetValues(typeof(StatType)))
        {
            _baseStats[type] = 0;
        }
    }
    private void InitializeOutsideDictionary()
    {
        foreach (StatType type in System.Enum.GetValues(typeof(StatType)))
        {
            _outsideStats[type] = 0;
        }
    }
    private void HandleDerivedStats(StatType type, int amount)
    {
        switch (type)
        {
            case StatType.Strength:
                ChangeStat(StatType.AttackPower, amount * GlobalData.STR_TO_ATK_RATIO);
                ChangeStat(StatType.MaxHealth, amount * GlobalData.STR_TO_HEALTH_RATIO);
                break;
            case StatType.Agility:
                ChangeStat(StatType.Armor, CountArmor(), false, true);
                ChangeStat(StatType.AttackSpeed, amount);
                break;
            case StatType.Intellect:
                ChangeStat(StatType.SpellPower, amount);
                ChangeStat(StatType.MaxMana, amount * GlobalData.INT_TO_MANA_RATIO);
                break;
        }
    }

    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<AddStrButtonPressedEvent>(StrButtonPress);
        _eventBus.Subscribe<AddAgiButtonPressedEvent>(AgiButtonPress);
        _eventBus.Subscribe<AddIntButtonPressedEvent>(IntButtonPress);
    }

    private void UnsubscribeFromEvents()
    {
        _eventBus.Unsubscribe<AddStrButtonPressedEvent>(StrButtonPress);
        _eventBus.Unsubscribe<AddAgiButtonPressedEvent>(AgiButtonPress);
        _eventBus.Unsubscribe<AddIntButtonPressedEvent>(IntButtonPress);
    }
    private void StrButtonPress(AddStrButtonPressedEvent e)
    {
        ChangeStat(StatType.Strength, 1);
    }
    private void AgiButtonPress(AddAgiButtonPressedEvent e)
    {
        ChangeStat(StatType.Agility, 1);
    }
    private void IntButtonPress(AddIntButtonPressedEvent e)
    {
        ChangeStat(StatType.Intellect, 1);
    }
    private int CountArmor()
    {
        int armorFormula = ((_baseStats[StatType.Agility] + _outsideStats[StatType.Agility]) / 5) + _defaultStats.Armor;
        return armorFormula;
    }
    public void InitDefaultStats(PlayerClassDefaultStatsSO stats)
    {
        _defaultStats = stats;
    }
    public void RestoreStats(PlayerSaveData data)
    {
        ChangeStat(StatType.MaxHealth, _defaultStats.DefaultHealth);
        ChangeStat(StatType.MaxMana, _defaultStats.DefaultMana);
        ChangeStat(StatType.AttackPower, _defaultStats.AttackPower);
        ChangeStat(StatType.SpellPower, _defaultStats.SpellPower);
        ChangeStat(StatType.AttackSpeed, _defaultStats.AttackSpeed);
        var d = data.StatsData;
        ChangeStat(StatType.Strength, d.StrenghtAmount);
        ChangeStat(StatType.Agility, d.AgilityAmount);
        ChangeStat(StatType.Intellect, d.IntellectAmount);
        ChangeStat(StatType.MoveSpeed, d.MoveSpeed);
        ChangeStat(StatType.Vampirism, d.Vampirism);
        ChangeStat(StatType.CriticalRate, d.CriticalRate);

    }
    public int GetBaseStat(StatType type)
    {
        return _baseStats[type];
    }

    public void ChangeStat(StatType type, int amount, bool silent = false, bool hardSet = false)
    {
        if (!hardSet)
        {
            _baseStats[type] += amount;
        }
        else
        {
            _baseStats[type] = amount;
        }  
        if (!silent)
            _eventBus.Publish(new StatChangedEvent(type, GetFinalStat(type)));
        HandleDerivedStats(type, amount);
    }
    public void ChangeOutsideStat(StatType type, int amount, bool silent = false)
    {
        _outsideStats[type] += amount;

        if (!silent)
            _eventBus.Publish(new StatChangedEvent(type, GetFinalStat(type)));
        HandleDerivedStats(type, amount);
    }

    public void SetDefaultStats()
    {
        ChangeStat(StatType.MaxHealth, _defaultStats.DefaultHealth);
        ChangeStat(StatType.MaxMana, _defaultStats.DefaultMana);
        ChangeStat(StatType.Strength, _defaultStats.Strenght);
        ChangeStat(StatType.Agility, _defaultStats.Agility);
        ChangeStat(StatType.Intellect, _defaultStats.Intellect);
        ChangeStat(StatType.AttackPower, _defaultStats.AttackPower);
        ChangeStat(StatType.Armor, CountArmor(), false, true);
        ChangeStat(StatType.SpellPower, _defaultStats.SpellPower);
        ChangeStat(StatType.MoveSpeed, _defaultStats.MoveSpeed);
        ChangeStat(StatType.Vampirism, _defaultStats.Vampirism);
        ChangeStat(StatType.AttackSpeed, _defaultStats.AttackSpeed);
        ChangeStat(StatType.CriticalRate, _defaultStats.CriticalRate);
    }
    public StatsSaveData GetStatsData()
    {
        var data = new StatsSaveData();
        data.StrenghtAmount = _baseStats[StatType.Strength];
        data.AgilityAmount = _baseStats[StatType.Agility];
        data.IntellectAmount = _baseStats[StatType.Intellect];
        data.MoveSpeed = _baseStats[StatType.MoveSpeed];
        data.Vampirism = _baseStats[StatType.Vampirism];
        data.CriticalRate = _baseStats[StatType.CriticalRate];

        return data;
    }
    public int GetFinalStat(StatType type)
    {
        return _baseStats[type] + _outsideStats[type];
    }
    public async UniTask ApplyTemporaryBonusAsync(string name,StatType type, int amount, float duration)
    {
        var bonus = new BonusEffect(name,type,amount, duration);

        _tempBonusEffects.Add(bonus);
        ChangeOutsideStat(type, amount);
        _eventBus.Publish(new PlayerBonusEffectAppliedEvent(name, duration));
        await RunBonusTimerAsync(bonus);
    }
    private void RemoveTemporaryBonusStat(BonusEffect bonus)
    {
        ChangeOutsideStat(bonus.StatType, -bonus.Amount);
        _tempBonusEffects.Remove(bonus);
    }
    private async UniTask RunBonusTimerAsync(BonusEffect bonus)
    {
        while (bonus.RemainingTime > 0f)
        {
            await UniTask.Yield();

            bonus.RemainingTime -= Time.deltaTime;
        }

        RemoveTemporaryBonusStat(bonus);
    }
    public BonusEffect[] GetBonusEffects()
    {
        return _tempBonusEffects.ToArray();
    }
}