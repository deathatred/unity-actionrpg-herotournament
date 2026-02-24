using Assets.Scripts.Core.Enums;
using Assets.Scripts.Core.Observer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InventoryViewUI : MonoBehaviour
{
    [Inject] private EventBus _eventBus;
    [SerializeField] private Sprite _emptySprite;
    [SerializeField] private InventorySlotSingleUI[] _inventorySlots;
    [SerializeField] private Image _selectedItemImage;
    [SerializeField] private TextMeshProUGUI _selectedItemName;
    [SerializeField] private TextMeshProUGUI _selectedItemAbout;

    [SerializeField] private Button _backButton;
    [SerializeField] private Button _equipButton;
    [SerializeField] private Button _useButton;
    [SerializeField] private Button _deleteButton;

    private InventorySlotSingleUI _selectedSlot;

    private void OnEnable()
    {
        ChangePreviewMenu(null);
        BindButtons();
        SubscribeToEvents();
    }
    private void Awake()
    {
        SetHolderInSlots();
    }
    private void OnDisable()
    {
        UnbindButtons();
        UnsubscribeFromEvents();
    }
    private void BindButtons()
    {
        _backButton.onClick.AddListener(BackPressed);
        _useButton.onClick.AddListener(UsePressed);
        _deleteButton.onClick.AddListener(DeletePressed);
    }
    private void UnbindButtons()
    {
        _backButton.onClick.RemoveListener(BackPressed);
        _useButton.onClick.RemoveListener(UsePressed);
        _deleteButton.onClick.RemoveListener(DeletePressed);
    }
    private void SetHolderInSlots()
    {
        foreach (var slot in _inventorySlots)
        {
            slot.SetHolder(this);
        }
    }
    private void BackPressed()
    {
        _eventBus.Publish(new BackButtonPressedEvent(BackButtonCaller.Inventory));
    }
    private void UsePressed()
    {
        _eventBus.Publish(new UseButtonPressedEvent());
    }
    private void DeletePressed()
    {
        _eventBus.Publish(new DeleteButtonPressedEvent(_selectedSlot.GetItemInstance()));
        ClearSelectedItemMenu();
    }
    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<CanvasChangedEvent>(ClearSlotSelection);
        _eventBus.Subscribe<ItemAddedToInventoryEvent>(AddItem);
        _eventBus.Subscribe<SlotButtonPressedEvent>(InventoryItemSelected);
        _eventBus.Subscribe<ItemClearedFromInventoryEvent>(ItemCleared);
        _eventBus.Subscribe<ItemAmountChangedEvent>(ItemAmountChanged);
    }
    private void UnsubscribeFromEvents()
    {
        _eventBus.Unsubscribe<CanvasChangedEvent>(ClearSlotSelection);
        _eventBus.Unsubscribe<ItemAddedToInventoryEvent>(AddItem);
        _eventBus.Unsubscribe<SlotButtonPressedEvent>(InventoryItemSelected);
        _eventBus.Unsubscribe<ItemClearedFromInventoryEvent>(ItemCleared);
        _eventBus.Unsubscribe<ItemAmountChangedEvent>(ItemAmountChanged);
    }
    private void ClearSlotSelection(CanvasChangedEvent e)
    {
        ClearItemPreview();
        _selectedSlot = null;
    }
    private void AddItem(ItemAddedToInventoryEvent e)
    {
        foreach (var slot in _inventorySlots)
        {
            if (e.Stacked && slot.GetItemInstance() == e.Item)
            {
                slot.SetItemAmount(e.Item.Amount);
                return;
            }
            if (!e.Stacked && !slot.IsOccupied)
            {
                slot.AddItem(e.Item);
                return;
            }
        }
    }
    private void InventoryItemSelected(SlotButtonPressedEvent e)
    {
        ChangePreviewMenu(e.SlotItem);
    }
    private void ItemCleared(ItemClearedFromInventoryEvent e)
    {
        ClearSelectedItemMenu();
    }
    private void ItemAmountChanged(ItemAmountChangedEvent e)
    {
        _selectedSlot.SetItemAmount(e.Amount);
    }
    private void ClearSelectedItemMenu()
    {
        if (_selectedSlot != null)
        {
            _selectedSlot.Clear();
            ClearItemPreview();
            _selectedSlot = null;
        }
    }
    private void ChangePreviewMenu(ItemInstance item)
    {
        if (item == null)
        {
            ClearItemPreview();
            return;
        }
        InventoryItemSO itemSO = item.Data;
        _selectedItemImage.sprite = itemSO.Icon;
        _selectedItemName.text = itemSO.ItemName;
        _selectedItemAbout.text = itemSO.About;
        SetItemButtons(itemSO.Equipable, itemSO.Usable, itemSO.Deletable);
    }
    private void ClearItemPreview()
    {
        _selectedItemImage.sprite = _emptySprite;
        _selectedItemName.text = "";
        _selectedItemAbout.text = "";

        SetItemButtons(false, false, false);
    }
    private void SetItemButtons(bool equip, bool use, bool delete)
    {
        _equipButton.interactable = equip;
        _useButton.interactable = use;
        _deleteButton.interactable = delete;
    }
    public void SetSelectedSlot(InventorySlotSingleUI slot)
    {
        _selectedSlot = slot;
    }
  
}
