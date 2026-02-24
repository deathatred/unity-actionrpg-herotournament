using Assets.Scripts.Core.Observer;
using UnityEngine;

public class PlayerConfiguredEvent : GameEventBase
{
    public PlayerClassSO PlayerClassSO{ get; private set; }
    public PlayerConfiguredEvent(PlayerClassSO playerClassSO)
    {
        PlayerClassSO = playerClassSO;
    }
}
