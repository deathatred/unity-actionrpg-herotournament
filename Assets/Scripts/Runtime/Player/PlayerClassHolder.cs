using UnityEngine;

public class PlayerClassHolder : MonoBehaviour
{
    public PlayerClass PlayerClass { get; private set; }
    private void SetClass(PlayerClass playerClass)
    {
        PlayerClass = playerClass;
    }
    public void RestoreClass(PlayerClass playerClass)
    {
        SetClass(playerClass);
    }
}
