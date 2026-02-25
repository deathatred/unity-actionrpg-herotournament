using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts
{
    public class InventoryItemSO : ScriptableObject
    {
        public string ItemID;
        public string ItemName;
        public string About;
        public Sprite Icon;
        public bool Stackable;
        public bool Usable;
        public bool Equipable;
        public bool Deletable;
        public int MaxStack = 99;
        public GameObject Prefab;
    }
}