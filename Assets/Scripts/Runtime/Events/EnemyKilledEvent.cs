using Assets.Scripts.Core.Observer;
using UnityEngine;

public class EnemyKilledEvent : GameEventBase
{
    public int XpAmount { get; private set; }
    public GameObject EnemyGameObject { get; private set; }
    public EnemyKilledEvent(int xpAmount, GameObject go)
    {
        XpAmount = xpAmount;
        EnemyGameObject = go;
    }
}
