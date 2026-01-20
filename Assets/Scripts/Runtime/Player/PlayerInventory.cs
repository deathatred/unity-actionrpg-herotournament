using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Zenject;

public class PlayerInventory : MonoBehaviour
{

    [Inject] private ItemsDatabase _itemsDatabase;
    [Inject] private EventBus _eventBus;
    private int _capacity = GlobalData.DEFAULT_INVENTORY_SIZE;
    public ItemInstance SelectedItem { get; private set; }
    private List<InventorySlot> _slots = new();

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void Awake()
    {
        for (int i = 0; i < _capacity; i++)
            _slots.Add(new InventorySlot());
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<PlayerFinishedCollectingEvent>(ItemCollectSubscriber);
        _eventBus.Subscribe<SlotButtonPressedEvent>(SelectItem);
        _eventBus.Subscribe<UseButtonPressedEvent>(ItemUsed);
        _eventBus.Subscribe<DeleteButtonPressedEvent>(FullyClearItem);
    }
    private void UnsubscribeFromEvents()
    {
        _eventBus.Unsubscribe<PlayerFinishedCollectingEvent>(ItemCollectSubscriber);
        _eventBus.Unsubscribe<SlotButtonPressedEvent>(SelectItem);
        _eventBus.Unsubscribe<UseButtonPressedEvent>(ItemUsed);
        _eventBus.Unsubscribe<DeleteButtonPressedEvent>(FullyClearItem);
    }
    private void ItemCollectSubscriber(PlayerFinishedCollectingEvent e)
    {
        AddItem(e.Item);
    }
    private void SelectItem(SlotButtonPressedEvent e)
    {
        SelectedItem = e.SlotItem;
    }
    private void ItemUsed(UseButtonPressedEvent e)
    {
        RemoveItem(SelectedItem);
    }
    private void FullyClearItem(DeleteButtonPressedEvent e)
    {
        ClearItem(e.Item);
    }
    public bool AddItem(ItemInstance item, int amount = 1) 
    {
        if (item.Data.Stackable)
        {
            foreach (var slot in _slots)
            {
                if (slot.item != null && slot.item.Data == item.Data)
                {
                    slot.item.Amount += amount;
                    _eventBus.Publish(new ItemAddedToInventoryEvent(true, slot.item));
                    return true;
                }
            }
        }
        foreach (var slot in _slots)
        {
            if (slot.item == null)
            {
                slot.item = item;
                slot.item.Amount = amount;
                _eventBus.Publish(new ItemAddedToInventoryEvent(false, slot.item));
                return true;
            }
        }

        return false;
    }
    public void RemoveItem(ItemInstance item)
    {
        foreach (var slot in _slots)
        {
            if (slot.item != null && slot.item == item)
            {
                if (item.Data.Stackable)
                {
                    slot.item.Amount--;
                    if (slot.item.Amount <= 0)
                    {
                        ClearItem(item);
                        return;
                    }
                    _eventBus.Publish(new ItemAmountChangedEvent(slot.item.Amount));
                }
            }
        }

    }
    public void ClearItem(ItemInstance item)
    {
        foreach (var slot in _slots)
        {
            if (slot.item == item)
            {
                slot.item = null;
                _eventBus.Publish(new ItemClearedFromInventoryEvent());
            }
        }
    }
    public InventoryItemsSaveData[] GetInventoryItemsData()
    {
        List<InventoryItemsSaveData> res = new();
        if (_slots == null) { return res.ToArray(); }
       
        foreach (var slot in _slots)
        { 
            if (slot.item != null)
            {
                var data = new InventoryItemsSaveData();
                data.ItemId = slot.item.Data.ItemID;
                data.Amount = slot.item.Amount;
                res.Add(data);
            }
        }
        return res.ToArray();
    }
    public void RestoreItems(InventoryItemsSaveData[] data)
    {
        if (data ==  null)
        {
            return;
        }
        foreach (var item in data)
        {
            var itemSO = _itemsDatabase.GetById(item.ItemId);
            var itemInstance = new ItemInstance(itemSO, "player_inventory");
            itemInstance.Amount = item.Amount;
            AddItem(itemInstance, itemInstance.Amount);
        }
    }
}

