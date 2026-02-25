using Assets.Scripts.Core.Enums;
using UnityEngine;

namespace Assets.Scripts.Runtime.Player
{
    public class PlayerClassHolder : MonoBehaviour
    {
        public PlayerClass PlayerClass { get; private set; }
        public void SetClass(PlayerClass playerClass)
        {
            PlayerClass = playerClass;
        }
    }
}