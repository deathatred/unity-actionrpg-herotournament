using UnityEngine;
using UnityEngine.UI;
using Zenject;

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
