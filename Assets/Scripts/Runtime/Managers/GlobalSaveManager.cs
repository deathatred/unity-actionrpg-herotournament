using Assets.Scripts.Core.Observer;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;


public class GlobalSaveManager : MonoBehaviour
{
    [Inject] private EventBus _eventBus;
    [Inject] private PlayerSaveManager _playerSaveManager;
    [Inject] private FirebaseManager _firebaseManager;
    [Inject] private LevelSaveManager _levelSaveManager;

    public bool GameLoaded = false;
    public bool _playerHasSaveFile;
    private CancellationTokenSource _cts;

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void Awake()
    {
        _cts = new CancellationTokenSource();
        CheckIfPlayerHasSaves().Forget();
    }
    private void OnDisable()
    {
        _cts.Cancel();
        _cts.Dispose();
        UnsubscribeFromEvents();
    }
    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<LevelInitedEvent>(SavePlayerDataSubscriber);
        _eventBus.Subscribe<RestartButtonPressedEvent>(RestartPressSubscriber);
        _eventBus.Subscribe<PlayButtonPressedEvent>(PlayPressSubscriber);
        _eventBus.Subscribe<ClassSelectedEvent>(ClassSelectedSubscriber);
    }
    private void UnsubscribeFromEvents()
    {
        _eventBus.Unsubscribe<LevelInitedEvent>(SavePlayerDataSubscriber);
        _eventBus.Unsubscribe<RestartButtonPressedEvent>(RestartPressSubscriber);
        _eventBus.Subscribe<PlayButtonPressedEvent>(PlayPressSubscriber); 
        _eventBus.Unsubscribe<ClassSelectedEvent>(ClassSelectedSubscriber);
    }
    private void SavePlayerDataSubscriber(LevelInitedEvent e)
    {
        var levelData = _levelSaveManager.CreateLevelSaveData();
        _firebaseManager.SaveLevelDataAsync(levelData).AttachExternalCancellation(_cts.Token).Forget();
    }
    private void RestartPressSubscriber(RestartButtonPressedEvent e)
    {
        ClearAllData().Forget();
    }
    private void PlayPressSubscriber(PlayButtonPressedEvent e)
    {
        if (_playerHasSaveFile)
        {
            InitAsync().Forget();
        }
    }
    private void ClassSelectedSubscriber(ClassSelectedEvent e)
    {
        InitAsync().Forget();
    }
    private async UniTask InitAsync()
    {
        await _playerSaveManager.LoadPlayerData();
        await _levelSaveManager.LoadLevelData();
        GameLoaded = true;
        StartAutoSaving().Forget();
    }

    private async UniTask StartAutoSaving()
    {
        while (!_cts.IsCancellationRequested)
        {
            var playerData = _playerSaveManager.CreatePlayerSaveData();
            await _firebaseManager.SavePlayerDataAsync(playerData).AttachExternalCancellation(_cts.Token);
            var levelData = _levelSaveManager.CreateLevelSaveData();
            await _firebaseManager.SaveLevelDataAsync(levelData).AttachExternalCancellation(_cts.Token);
            await UniTask.WaitForSeconds(5f, cancellationToken: _cts.Token);
        }
    }
    public async UniTask<bool> CheckIfPlayerHasSaves()
    {
        var playerData = await _firebaseManager.LoadPlayerDataAsync().AttachExternalCancellation(_cts.Token);
        var levelData = await _firebaseManager.LoadLevelDataAsync().AttachExternalCancellation(_cts.Token);
        return _playerHasSaveFile = (playerData != null) && (levelData != null);
    }
    public async UniTask ClearLevelDataAndSaveNew()
    {
        var playerData = _playerSaveManager.CreatePlayerSaveData();
        var levelData = _levelSaveManager.CreateLevelSaveData();
        await _firebaseManager.SaveNewLevelData(levelData, playerData);
    }
    public async UniTask ClearAllData()
    {
        await _firebaseManager.ClearAllData();
    } 
}
