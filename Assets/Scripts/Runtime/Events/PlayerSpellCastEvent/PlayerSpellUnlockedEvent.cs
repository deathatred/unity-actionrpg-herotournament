using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.SOScripts.SpellSOs;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events.PlayerSpellCastEvent
{
    public class PlayerSpellUnlockedEvent : GameEventBase
    {
        public SpellSO SpellSO { get; private set; }
        public PlayerSpellUnlockedEvent(SpellSO spellSO)
        {
            SpellSO = spellSO;
        }
    }
}