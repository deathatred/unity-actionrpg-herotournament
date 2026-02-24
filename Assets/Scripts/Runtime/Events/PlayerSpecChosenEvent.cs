using Assets.Scripts.Core.Observer;
using UnityEngine;

public class PlayerSpecChosenEvent : GameEventBase
{
    public ClassSpecSO Spec;
    public PlayerSpecChosenEvent(ClassSpecSO spec)
    {
       Spec = spec;
    }
}
