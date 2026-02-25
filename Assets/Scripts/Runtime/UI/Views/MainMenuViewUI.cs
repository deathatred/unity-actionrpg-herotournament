using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.UI.UIEvents;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Runtime.UI.Views
{
    public class MainMenuViewUI : MonoBehaviour
    {
        [Inject] private EventBus _eventBus;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;

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
            _playButton.onClick.AddListener(PlayButtonPressed);
        }
        private void UnbindButtons()
        {
            _playButton.onClick.RemoveListener(PlayButtonPressed);
        }
        private void SubscribeToEvents()
        {

        }
        private void UnsubscribeFromEvents()
        {

        }
        private void PlayButtonPressed()
        {
            _eventBus.Publish(new PlayButtonPressedEvent());
        }
    }
}