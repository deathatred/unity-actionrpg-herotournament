using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.UI.UIEvents;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Runtime.UI.Views
{
    public class DeadMenuViewUI : MonoBehaviour
    {
        [Inject] private EventBus _eventBus;
        [SerializeField] private Button _restartButton;

        private void OnEnable()
        {
            BindButtons();
        }
        private void OnDisable()
        {
            UnbindButton();
        }
        private void BindButtons()
        {
            _restartButton.onClick.AddListener(RestartPress);
        }
        private void UnbindButton()
        {
            _restartButton.onClick.RemoveListener(RestartPress);
        }
        private void RestartPress()
        {
            _eventBus.Publish(new RestartButtonPressedEvent());
        }
    }
}