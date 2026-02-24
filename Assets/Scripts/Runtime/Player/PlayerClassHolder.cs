using Assets.Scripts.Core.Enums;
using UnityEngine;

public class PlayerClassHolder : MonoBehaviour
{
    public PlayerClass PlayerClass { get; private set; }
    public void SetClass(PlayerClass playerClass)
    {
        PlayerClass = playerClass;
    }
}
