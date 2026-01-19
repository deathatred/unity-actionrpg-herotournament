using UnityEngine;

public class PlayerDataLoadedEvent : GameEventBase
{
    public TalentSaveData[] TalentSaveData;
    public PlayerDataLoadedEvent(TalentSaveData[] talentSaveData)
    {
        TalentSaveData = talentSaveData;
    }
}
