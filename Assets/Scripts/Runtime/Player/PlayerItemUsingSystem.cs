using UnityEngine;
using Zenject;

public class PlayerItemUsingSystem : MonoBehaviour
{
    [Inject] private PlayerInventory _playerInventory;
    [Inject] private PlayerHealthSystem _playerHealthSystem;
    [Inject] private EventBus _eventBus;

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<UseButtonPressedEvent>(UseSelectedItem);
    }
    private void UnsubscribeFromEvents()
    {
        _eventBus.Unsubscribe<UseButtonPressedEvent>(UseSelectedItem);
    }
    private void UseSelectedItem(UseButtonPressedEvent e)
    {
        var item = _playerInventory.SelectedItem;
        if (item == null)
            return;

        if (item.Data is IUsable usable)
        {
            print("used");
            usable.Use(_playerHealthSystem);
        }
        else
        {
            print("Item is not usable");
        }
    }
}
    
