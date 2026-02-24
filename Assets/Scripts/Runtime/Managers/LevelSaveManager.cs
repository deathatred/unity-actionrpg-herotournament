using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

public class LevelSaveManager : IDisposable
{
    [Inject] private FirebaseManager _firebaseManager;
    private EventBus _eventBus;
    private readonly GameManager _gameManager;
    private bool _playerConfigured;
    public LevelSaveManager(GameManager gameManager, EventBus eventBus)
    {
        _gameManager = gameManager;
        _eventBus = eventBus;
        _eventBus.Subscribe<PlayerConfiguredEvent>(PlayerConfiguredSubscriber);
    }
    private void PlayerConfiguredSubscriber(PlayerConfiguredEvent e)
    {
        _playerConfigured = true;
    }
    public LevelSaveData CreateLevelSaveData()
    {
        var level = _gameManager.CurrentLevel;
        var data = new LevelSaveData();
        data.LevelIndex = _gameManager.CurrentLevelIndex;
        data.IsSurvivalMode = level.GetLevelConfig().LevelType == LevelType.Survival ? true : false;
        if (data.IsSurvivalMode)
        {
            data.SurvivalTimeLeft = level.Timer;
            data.TimeToNextSpawn = level.TimeToNextSpawn;
        }
        else
        {
            data.AmountOfEnemiesLeft = level.AmountOfEnemies;
        }
        data.ItemsSceneIDs = level.GetPickedUpItems();
        data.Enemies = level.CreateEnemySpawnData();
        return data;
    }
    public async UniTask LoadLevelData()
    {
        var data = await _firebaseManager.LoadLevelDataAsync();
        await UniTask.WaitUntil(() => _playerConfigured == true);
        await UniTask.WaitForSeconds(1f);
        if (data == null)
        {
             await _gameManager.InitStartingLevelAsync();
            _eventBus.Publish(new LevelInitedEvent());
            return;
        }
        _gameManager.RestoreLevelPrefab(data.LevelIndex);
        
        _gameManager.CurrentLevel.RestoreLevel(data);
        _eventBus.Publish(new LevelInitedEvent());
    }

    public void Dispose()
    {
        _eventBus.Unsubscribe<PlayerConfiguredEvent>(PlayerConfiguredSubscriber);
    }
}
