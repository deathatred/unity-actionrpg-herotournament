using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.Events;
using Assets.Scripts.Runtime.Events.GameLevelEvents;
using Assets.Scripts.Runtime.Events.PlayerLevelSystemEvents;
using Assets.Scripts.Runtime.UI.UIEvents;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Runtime.Player
{
    public class PlayerLevelSystem : MonoBehaviour
    {
        [Inject] private EventBus _eventBus;
        [Inject] private PlayerTalentSystem _playerTalent;
        public int CurrentLevel { get; private set; }
        public int LevelPoints { get; private set; }
        public int CurrentXp { get; private set; }
        public int XpToNextLevel { get; private set; }
        private int _levelStep = 30;

        private void OnEnable()
        {
            SubscribeToEvents();
        }
        private void Awake()
        {
            CurrentLevel = 1;
            XpToNextLevel = 30;
        }
        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }
        private void SubscribeToEvents()
        {
            _eventBus.Subscribe<EnemyKilledEvent>(EnemyKilled);
            _eventBus.Subscribe<AddStrButtonPressedEvent>(AddStrenghtSubscriber);
            _eventBus.Subscribe<AddAgiButtonPressedEvent>(AddAgilitySubscriber);
            _eventBus.Subscribe<AddIntButtonPressedEvent>(AddIntellectSubscriber);
        }
        private void UnsubscribeFromEvents()
        {
            _eventBus.Unsubscribe<EnemyKilledEvent>(EnemyKilled);
            _eventBus.Unsubscribe<AddStrButtonPressedEvent>(AddStrenghtSubscriber);
            _eventBus.Unsubscribe<AddAgiButtonPressedEvent>(AddAgilitySubscriber);
            _eventBus.Unsubscribe<AddIntButtonPressedEvent>(AddIntellectSubscriber);
        }
        private void EnemyKilled(EnemyKilledEvent e)
        {
            AddXpAsync(e.XpAmount).Forget();
        }
        private void AddStrenghtSubscriber(AddStrButtonPressedEvent e)
        {
            SpendLevelPoint();
        }
        private void AddAgilitySubscriber(AddAgiButtonPressedEvent e)
        {
            SpendLevelPoint();
        }
        private void AddIntellectSubscriber(AddIntButtonPressedEvent e)
        {
            SpendLevelPoint();
        }
        private void LevelUp()
        {
            CurrentLevel++;
            CurrentXp = CurrentXp - XpToNextLevel;
            XpToNextLevel += _levelStep + CurrentLevel * 25;
            LevelPoints++;
            _playerTalent.AddTalentPoint();
            _eventBus.Publish(new PlayerLevelChangedEvent(CurrentLevel));
        }

        private void SpendLevelPoint()
        {
            --LevelPoints;
            _eventBus.Publish(new PlayerLevelPointsSpentEvent(LevelPoints));
        }
        public void RestorePlayerLevelData(int level, int levelPoints, int currentXP, int XPtoNextLevel)
        {
            CurrentLevel = level;
            LevelPoints = levelPoints;
            CurrentXp = currentXP;
            XpToNextLevel = XPtoNextLevel;
            _eventBus.Publish(new PlayerLevelRestoredEvent(CurrentLevel, LevelPoints, CurrentXp, XpToNextLevel));
        }
        private async UniTask AddXpAsync(int amount)
        {
            CurrentXp += amount;
            float levelFillingTime = 0.35f;
            while (CurrentXp >= XpToNextLevel)
            {
                LevelUp();
                await UniTask.WaitForSeconds(levelFillingTime);
            }
            _eventBus.Publish(new PlayerGainedXpEvent(CurrentXp, XpToNextLevel));
        }
    }
}