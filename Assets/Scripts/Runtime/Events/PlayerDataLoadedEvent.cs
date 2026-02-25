using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Observer;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events
{
    public class PlayerDataLoadedEvent : GameEventBase
    {
        public TalentSaveData[] TalentSaveData;
        public PlayerDataLoadedEvent(TalentSaveData[] talentSaveData)
        {
            TalentSaveData = talentSaveData;
        }
    }
}