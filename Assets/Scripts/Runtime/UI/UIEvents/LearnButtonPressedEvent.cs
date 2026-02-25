using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.SOScripts;
using UnityEngine;

namespace Assets.Scripts.Runtime.UI.UIEvents
{
    public class LearnButtonPressedEvent : GameEventBase
    {
        public TalentSO TalentSO { get; private set; }
        public LearnButtonPressedEvent(TalentSO talentSO)
        {
            TalentSO = talentSO;
        }
    }
}