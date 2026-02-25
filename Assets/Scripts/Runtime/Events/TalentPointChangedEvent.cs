using Assets.Scripts.Core.Observer;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events
{
    public class TalentPointChangedEvent : GameEventBase
    {
        public int TalentPoints { get; private set; }
        public TalentPointChangedEvent(int talentPoints)
        {
            TalentPoints = talentPoints;
        }
    }
}