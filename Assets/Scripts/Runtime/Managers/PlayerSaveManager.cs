using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class PlayerSaveManager
{
    [Inject] private FirebaseManager _firebaseManager;

    private readonly EventBus _eventBus;
    private readonly PlayerStats _stats;
    private readonly PlayerTalentSystem _talents;
    private readonly PlayerInventory _inventory;
    private readonly PlayerLevelSystem _levelSystem;
    private readonly PlayerController _playerController;
    private readonly PlayerHealthSystem _healthSystem;
    private readonly PlayerSpellCasting _spellCasting;

    public PlayerSaveManager(
        EventBus eventBus,
        PlayerStats stats,
        PlayerTalentSystem talents,
        PlayerHealthSystem healthSystem,
        PlayerSpellCasting spellCasting,
        PlayerInventory inventory,
        PlayerLevelSystem levelSystem,
        PlayerController playerController)
    {
        _eventBus = eventBus;
        _stats = stats;
        _talents = talents;
        _inventory = inventory;
        _levelSystem = levelSystem;
        _playerController = playerController;
        _healthSystem = healthSystem;
        _spellCasting = spellCasting;
    }
    public PlayerSaveData CreatePlayerSaveData()
    {
        return new PlayerSaveData
        {
            CurrentLevel = _levelSystem.CurrentLevel,
            ExpAmount = _levelSystem.CurrentXp,
            ExpRequired = _levelSystem.XpToNextLevel,
            TalentPointsAmount = _talents.TalentPoints,
            LevelPointsAmount = _levelSystem.LevelPoints,
            StatsData = _stats.GetStatsData(),
            CurrentHealth = _healthSystem.Health,
            CurrentMana = _spellCasting.CurrentMana,
            LearnedTalentsIds = _talents.GetLearnedTalents(),
            InventoryItems = _inventory.GetInventoryItemsData(),
            Position = _playerController.transform.position
        };
    }
    public async UniTask LoadPlayerData()
    {
        var data = await _firebaseManager.LoadPlayerDataAsync();
        if (data == null)
        {
            Debug.Log("default set");
            _stats.SetDefaultStats();
            return;
        }

        _levelSystem.RestorePlayerLevelData(data.CurrentLevel, data.LevelPointsAmount, data.ExpAmount,data.ExpRequired);
        _stats.RestoreStats(data);
        _talents.RestoreTalentPoints(data.TalentPointsAmount);

        _talents.RestoreTalents(data.LearnedTalentsIds);

        _inventory.RestoreItems(data.InventoryItems);

        _healthSystem.RestoreHealth(data.CurrentHealth);
        _spellCasting.RestorePlayerMana(data.CurrentMana);

        _eventBus.Publish(new PlayerDataLoadedEvent(data.LearnedTalentsIds));
        _playerController.WarpToPosition(data.Position);
    }
}
