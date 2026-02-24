using Assets.Scripts.Core.Observer;
using UnityEngine;

public class TalentContainerPressedEvent : GameEventBase
{
    public TalentSO TalentSO { get; private set;}
    public TalentContainerPressedEvent(TalentSO talentSO)
    {
        TalentSO = talentSO;
    }
}
