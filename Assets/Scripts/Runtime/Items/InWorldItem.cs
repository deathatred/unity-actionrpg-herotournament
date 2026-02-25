using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.Events;
using Assets.Scripts.Runtime.SOScripts;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Runtime.Items
{
    public class InWorldItem : MonoBehaviour
    {
        [Inject] private EventBus _eventBus;
        [SerializeField] private InventoryItemSO _inventoryItemSO;
        public string SceneID { get; private set; }
        public void Collect()
        {
            _eventBus.Publish(new PlayerFinishedCollectingEvent(new ItemInstance(GetInventoryItemSO(), SceneID)));
            Destroy(gameObject);
        }
        public InventoryItemSO GetInventoryItemSO()
        {
            return _inventoryItemSO;
        }
        public void SetSceneID(string number)
        {
            SceneID = number;
        }
    }
}