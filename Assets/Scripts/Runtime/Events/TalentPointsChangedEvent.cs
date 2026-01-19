using UnityEngine;

public class TalentPointsChangedEvent : GameEventBase
{
    public int TalentPoints { get; private set; }
    public TalentPointsChangedEvent(int points)
    {
        TalentPoints = points;
    }
}
