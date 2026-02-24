using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class InventorySlotSingleUI : MonoBehaviour
{
    [Inject] private EventBus _eventBus;
    [SerializeField] private Sprite _emptySprite;
    [SerializeField] private Button _slotButton;
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemAmount;
    [SerializeField] private int _id;
    private ItemInstance _item;
    private InventoryViewUI _holder;

    public bool IsOccupied { get; private set; }    

    private void OnEnable()
    {
        BindButton();
    }
    private void OnDisable()
    {
        UnbindButton();
    }
    private void BindButton()
    {
        _slotButton.onClick.AddListener(SlotPressed);
    }
    private void UnbindButton()
    {
        _slotButton.onClick.RemoveListener(SlotPressed);
    }
    private void SlotPressed()
    {
        _holder.SetSelectedSlot(this);
        _eventBus.Publish(new SlotButtonPressedEvent(_item));
    }
    public void AddItem(ItemInstance item)
    {
        InventoryItemSO itemSO = item.Data;
        _itemImage.sprite = itemSO.Icon;
        IsOccupied = true;
        _item = item;
        _itemAmount.text = item.Amount.ToString();
    }
    public void SetItemAmount(int amount)
    {
        _itemAmount.text = amount.ToString();
    }
    public void Clear()
    {
        _item = null;
        _itemImage.sprite = _emptySprite;
        _itemAmount.text = string.Empty;
    }
    public void SetHolder(InventoryViewUI view)
    {
        _holder = view;
    }
    public ItemInstance GetItemInstance()
    {
        return _item;
    }
}
