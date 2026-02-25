using Assets.Scripts.Runtime.SOScripts;
using UnityEngine;

namespace Assets.Scripts.Runtime.Items
{
    [System.Serializable]
    public class ItemInstance
    {
        public InventoryItemSO Data;
        public int Amount = 1;
        public int Durability;
        public string SceneID;

        public ItemInstance(InventoryItemSO so, string sceneID)
        {
            Data = so;
            Amount = 1;
            SceneID = sceneID;
        }
    }
}