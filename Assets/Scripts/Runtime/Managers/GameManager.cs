using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.LevelsLogic;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private EventBus _eventBus;
    [Inject] private GlobalSaveManager _globalSaveManager;
    [SerializeField] private List<Level> _levelsList;
    public Level CurrentLevel { get; private set; }
    public bool IsPaused { get; private set; } 
    public int CurrentLevelIndex { get; private set; }
    private void OnEnable()
    {
        SubscribeToEvents();

    }
    private void Start()
    {
        Pause();
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<PortalInteractedEvent>(LoadNextLevel);
        //_eventBus.Subscribe<PlayButtonPressedEvent>(StartingLevel);
        _eventBus.Subscribe<PlayerDeadEvent>(PlayerDeadSubscriber);
        _eventBus.Subscribe<RestartButtonPressedEvent>(RestartPress);
        
    }
    private void UnsubscribeFromEvents()
    {
        _eventBus.Unsubscribe<PortalInteractedEvent>(LoadNextLevel);
        //_eventBus.Unsubscribe<PlayButtonPressedEvent>(StartingLevel);
        _eventBus.Subscribe<PlayerDeadEvent>(PlayerDeadSubscriber);
        _eventBus.Unsubscribe<RestartButtonPressedEvent>(RestartPress);
    }
    private void LoadNextLevel(PortalInteractedEvent e)
    {
        LoadNextLevelAsync().Forget();
    }
    private void StartingLevel(PlayButtonPressedEvent e)
    {
        LoadStartLevelAsync().Forget();
    }
    private void PlayerDeadSubscriber(PlayerDeadEvent e)
    {
        Time.timeScale = 0f;
    }
    private void RestartPress(RestartButtonPressedEvent e)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public async UniTask InitStartingLevelAsync()
    {
        CurrentLevel = _levelsList[0];
        CurrentLevel.InitLevel();
        _eventBus.Publish(new LevelLoadedEvent(CurrentLevel.GetPlayerSpawnPoint()));
        await UniTask.WaitForFixedUpdate();
    }
    public void RestoreLevelPrefab(int index)
    {
        CurrentLevelIndex = index;
        CurrentLevel = _levelsList[CurrentLevelIndex];
    }
    private void Pause()
    {
        IsPaused = true;
    }
    private void Unpause()
    {
        IsPaused = false; 
    }
    public async UniTask LoadStartLevelAsync()
    {
        await UniTask.WaitUntil(() => _globalSaveManager.GameLoaded);
        Unpause();
    }
    public async UniTask LoadNextLevelAsync()
    {
        Pause();
        CurrentLevel.gameObject.SetActive(false);
        CurrentLevelIndex++;
        CurrentLevel = _levelsList[CurrentLevelIndex];
        CurrentLevel.InitLevel();
        _eventBus.Publish(new LevelLoadedEvent(CurrentLevel.GetPlayerSpawnPoint()));
        await UniTask.WaitForFixedUpdate();
        await _globalSaveManager.ClearLevelDataAndSaveNew();
        _eventBus.Publish(new LevelInitedEvent());
        await UniTask.WaitForSeconds(1f, true);

        Unpause();
    }
}
