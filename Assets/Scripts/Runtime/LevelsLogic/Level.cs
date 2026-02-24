using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Observer;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Runtime.LevelsLogic
{
    public class Level : MonoBehaviour
    {
        [Inject] private AudioManager _audioManager;
        [Inject] private EventBus _eventBus;
        [Inject] private EnemySpawnManager _enemySpawnManager;
        [Inject] private ItemSpawnManager _itemSpawnManager;

        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private List<Transform> _enemySpawnPoints;
        [SerializeField] private List<ItemSpawnData> _itemsSpawnData;
        [SerializeField] private Portal _levelPortal;
        [SerializeField] private List<Transform> _patrolPoints;
        [SerializeField] private LevelConfigSO _levelConfigSO;

        public int Timer { get; private set; }
        public int TimeToNextSpawn { get; private set; }
        public int AmountOfEnemies { get; private set; }

        private readonly List<GameObject> _spawnedEnemies = new();
        private readonly List<string> _pickedUpItems = new();


        private void Awake()
        {
            AmountOfEnemies = _levelConfigSO.Wave.Enemies.Count;
        }

        private void OnEnable()
        {
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            DestroyAllEnemies();
            UnsubscribeFromEvents();
        }


        private void SubscribeToEvents()
        {
            _eventBus.Subscribe<EnemyKilledEvent>(OnEnemyKilled);
            _eventBus.Subscribe<PlayerFinishedCollectingEvent>(OnItemCollected);
        }

        private void UnsubscribeFromEvents()
        {
            _eventBus.Unsubscribe<EnemyKilledEvent>(OnEnemyKilled);
            _eventBus.Unsubscribe<PlayerFinishedCollectingEvent>(OnItemCollected);
        }

        private void SpawnDefaultEnemies()
        {
            for (int i = 0; i < _levelConfigSO.Wave.Enemies.Count; i++)
            {
                var enemy = _enemySpawnManager.InitEnemy(
                    _enemySpawnPoints[i].position,
                    _levelConfigSO.Wave.Enemies[i]
                );

                _spawnedEnemies.Add(enemy);
            }
        }

        private void DestroyAllEnemies()
        {
            foreach (var enemy in _spawnedEnemies)
            {
                if (enemy != null)
                    Destroy(enemy);
            }

            _spawnedEnemies.Clear();
        }

        private void OnEnemyKilled(EnemyKilledEvent e)
        {
            AmountOfEnemies--;
            _spawnedEnemies.Remove(e.EnemyGameObject);

            if (AmountOfEnemies <= 0 && _levelConfigSO.LevelType == LevelType.Default)
                _levelPortal.EnablePortal();

            if (_levelConfigSO.LevelType == LevelType.Default)
                _eventBus.Publish(new AmountOfMobsOnLevelDecreasedEvent(AmountOfEnemies));
        }

        private void OnItemCollected(PlayerFinishedCollectingEvent e)
        {
            _pickedUpItems.Add(e.Item.SceneID);
        }

        private void PublishDefaultInitEvents()
        {
            _eventBus.Publish(new DefaultModeStartedEvent(AmountOfEnemies));
        }

        private void RestorePickedItems(LevelSaveData data)
        {
            _pickedUpItems.Clear();

            if (data.ItemsSceneIDs != null)
                _pickedUpItems.AddRange(data.ItemsSceneIDs);
        }

        private void RestoreEnemies(LevelSaveData data)
        {
            if (data.Enemies == null)
                return;

            foreach (var enemyData in data.Enemies)
            {
                var enemy = _enemySpawnManager.RestoreEnemy(enemyData);
                _spawnedEnemies.Add(enemy);
            }
        }

        private void RestoreItems(LevelSaveData data)
        {
            var collected = data.ItemsSceneIDs != null
                ? new HashSet<string>(data.ItemsSceneIDs)
                : new HashSet<string>();

            var itemsToSpawn = new List<ItemSpawnData>();

            foreach (var spawnData in _itemsSpawnData)
            {
                if (!collected.Contains(spawnData.SpawnIndex))
                    itemsToSpawn.Add(spawnData);
            }

            _itemSpawnManager.SpawnItems(itemsToSpawn, transform);
        }
        private void StartNewSurvival()
        {
            Timer = _levelConfigSO.SurvivalDuration;
            TimeToNextSpawn = _levelConfigSO.SpawnInterval;
            StartSurvival();
        }
        private void StartSurvival()
        {
            _eventBus.Publish(new SurvivalModeStartedEvent(Timer));

            StartSurvivalTimer().Forget();
        }


        public void InitLevel()
        {
            gameObject.SetActive(true);

            _itemSpawnManager.SpawnItems(_itemsSpawnData, transform);

            if (_levelConfigSO.LevelType == LevelType.Default)
            {
                SpawnDefaultEnemies();
                PublishDefaultInitEvents();
            }
            else if (_levelConfigSO.LevelType == LevelType.Survival)
            {
                StartNewSurvival();
            }

            _audioManager.PlayAmbient(_levelConfigSO._levelAmbient);
        }

        public void RestoreLevel(LevelSaveData data)
        {
            gameObject.SetActive(true);

            RestorePickedItems(data);
            RestoreEnemies(data);
            RestoreItems(data);

            if (_levelConfigSO.LevelType == LevelType.Default)
            {
                AmountOfEnemies = data.AmountOfEnemiesLeft;
                PublishDefaultInitEvents();
            }
            else if (_levelConfigSO.LevelType == LevelType.Survival)
            {
                Timer = data.SurvivalTimeLeft;
                TimeToNextSpawn = data.TimeToNextSpawn;
                StartSurvival();
            }

            if (AmountOfEnemies == 0 && _levelConfigSO.LevelType == LevelType.Default)
            {
                _levelPortal.EnablePortal();
                _eventBus.Publish(new AmountOfMobsOnLevelDecreasedEvent(AmountOfEnemies));
            }
            else if (Timer == 0 && _levelConfigSO.LevelType == LevelType.Survival)
            {
                _levelPortal.EnablePortal();
            }
            _audioManager.PlayAmbient(_levelConfigSO._levelAmbient);
        }

        public EnemySaveData[] CreateEnemySpawnData()
        {
            var data = new List<EnemySaveData>();

            foreach (var enemy in _spawnedEnemies)
            {
                data.Add(new EnemySaveData
                {
                    EnemyId = enemy.GetComponent<EnemyData>().GetEnemyData().ID,
                    Position = enemy.transform.position,
                    CurrentHealth = enemy.GetComponent<IHealthSystem>().Health
                });
            }

            return data.ToArray();
        }

        public string[] GetPickedUpItems() => _pickedUpItems.ToArray();
        public List<Transform> GetPatrolPoints() => _patrolPoints;
        public LevelConfigSO GetLevelConfig() => _levelConfigSO;
        public Transform GetPlayerSpawnPoint() => _playerSpawnPoint;

        private async UniTaskVoid StartSurvivalTimer()
        {
            var token = this.GetCancellationTokenOnDestroy();
            while (Timer > 0)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);

                Timer--;
                TimeToNextSpawn--;

                if (TimeToNextSpawn <= 0)
                {
                    SpawnSurvivalEnemy();
                    TimeToNextSpawn = _levelConfigSO.SpawnInterval;
                }
            }

            _levelPortal.EnablePortal();
        }
        private void SpawnSurvivalEnemy()
        {
            if (_enemySpawnPoints.Count == 0 || _levelConfigSO.Wave.Enemies.Count == 0)
            {
                Debug.LogError("Survival misconfigured");
                return;
            }

            int enemyCount = _levelConfigSO.Wave.Enemies.Count;

            var enemy = _enemySpawnManager.InitEnemy(
                _enemySpawnPoints[UnityEngine.Random.Range(0, _enemySpawnPoints.Count)].position,
                _levelConfigSO.Wave.Enemies[UnityEngine.Random.Range(0, enemyCount)]
            );

            _spawnedEnemies.Add(enemy);
        }

    }
}