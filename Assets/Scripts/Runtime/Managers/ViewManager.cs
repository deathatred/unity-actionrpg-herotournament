using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

public class ViewManager : IDisposable
{
    private Canvas[] _views;
    private EventBus _eventBus;
    private CancellationTokenSource _cts;
    [Inject]
    public ViewManager(Canvas[] views, EventBus eventBus)
    {
        _views = views;
        _eventBus = eventBus;
        _cts = new CancellationTokenSource();
        SubscribeToEvents();
    }
    private void ChangeCanvas(int id)
    {
        if (id != 0)
        {
            InputHandler.DisablePlayerInput();
        }
        else
        {
            InputHandler.EnablePlayerInput();
        }
        foreach (var view in _views)
        {
            view.enabled = false;
        }

        _views[id].enabled = true;
        _eventBus.Publish(new CanvasChangedEvent(id));
    }
    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<BackButtonPressedEvent>(BackButtonPressed);
        _eventBus.Subscribe<MenuButtonPressedEvent>(MenuButtonPressed);
        _eventBus.Subscribe<InventoryPressedEvent>(InventoryButtonPressed);
        _eventBus.Subscribe<TalentTreeButtonPressedEvent>(TalentTreeButtonPress);
        _eventBus.Subscribe<PlayButtonPressedEvent>(PlayButtonPressedEvent);
        _eventBus.Subscribe<LevelInitedEvent>(LevelInited);
        _eventBus.Subscribe<PortalInteractedEvent>(PortalInteracted);
        _eventBus.Subscribe<PlayerDeadEvent>(PlayerDead);
    }
    private void UnsubscribeFromEvents()
    {
        _eventBus.Unsubscribe<BackButtonPressedEvent>(BackButtonPressed);
        _eventBus.Unsubscribe<MenuButtonPressedEvent>(MenuButtonPressed);
        _eventBus.Unsubscribe<InventoryPressedEvent>(InventoryButtonPressed);
        _eventBus.Unsubscribe<TalentTreeButtonPressedEvent>(TalentTreeButtonPress);
        _eventBus.Unsubscribe<PlayButtonPressedEvent>(PlayButtonPressedEvent);
        _eventBus.Unsubscribe<LevelInitedEvent>(LevelInited);
        _eventBus.Unsubscribe<PortalInteractedEvent>(PortalInteracted);
        _eventBus.Unsubscribe<PlayerDeadEvent>(PlayerDead);
    }
    private void LevelInited(LevelInitedEvent e)
    {
        ChangeToGameViewAfterDelay(_cts).Forget();
    } 
    private void BackButtonPressed(BackButtonPressedEvent e)
    {
        switch (e.Caller)
        {
            case BackButtonCaller.Inventory:
                ChangeCanvas(1);
                break;
            case BackButtonCaller.TalentTree: 
                ChangeCanvas(1); 
                break;
            case BackButtonCaller.StatsMenu:
                ChangeCanvas(0);
                break;
        }
    }
    private void MenuButtonPressed(MenuButtonPressedEvent e)
    {
        ChangeCanvas(1);
    }
    private void InventoryButtonPressed(InventoryPressedEvent e)
    {
        ChangeCanvas(2);
    }
    private void TalentTreeButtonPress(TalentTreeButtonPressedEvent e)
    {
        ChangeCanvas(3);
    }
    private void PlayButtonPressedEvent(PlayButtonPressedEvent e)
    {
        ChangeCanvas(5);
    } 
    private void PortalInteracted(PortalInteractedEvent e)
    {
        ChangeCanvas(5);
    }
    private void PlayerDead(PlayerDeadEvent e)
    {
        ChangeCanvas(6);
    }
    public void Dispose()
    {
        UnsubscribeFromEvents();
        _cts.Cancel();
        _cts.Dispose();
    }
    private async UniTask ChangeToGameViewAfterDelay(CancellationTokenSource cts)
    {
        await UniTask.WaitForSeconds(1f).AttachExternalCancellation(cts.Token);
        ChangeCanvas(0);
    }

}
