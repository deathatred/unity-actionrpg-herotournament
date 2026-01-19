using UnityEngine;

public class LearnButtonPressedEvent : GameEventBase
{
    public TalentSO TalentSO { get; private set; }
    public LearnButtonPressedEvent(TalentSO talentSO)
    {
        TalentSO = talentSO;
    }
}
