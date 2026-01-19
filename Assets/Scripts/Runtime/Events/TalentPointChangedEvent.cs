using UnityEngine;

public class TalentPointChangedEvent : GameEventBase
{
    public int TalentPoints {  get; private set; }
    public TalentPointChangedEvent(int talentPoints)
    {
        TalentPoints = talentPoints;
    }
}
