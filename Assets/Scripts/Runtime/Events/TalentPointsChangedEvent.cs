using Assets.Scripts.Core.Observer;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events
{
    public class TalentPointsChangedEvent : GameEventBase
    {
        public int TalentPoints { get; private set; }
        public TalentPointsChangedEvent(int points)
        {
            TalentPoints = points;
        }
    }
}