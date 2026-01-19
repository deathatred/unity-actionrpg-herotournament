using UnityEngine;

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
