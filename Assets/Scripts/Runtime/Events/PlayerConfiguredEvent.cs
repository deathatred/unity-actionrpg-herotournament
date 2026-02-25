using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.SOScripts.ClassSO;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events
{
    public class PlayerConfiguredEvent : GameEventBase
    {
        public PlayerClassSO PlayerClassSO { get; private set; }
        public PlayerConfiguredEvent(PlayerClassSO playerClassSO)
        {
            PlayerClassSO = playerClassSO;
        }
    }
}