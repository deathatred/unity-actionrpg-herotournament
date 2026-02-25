using Assets.Scripts.Core.Observer;
using Assets.Scripts.Runtime.SOScripts.SpellSOs;
using UnityEngine;

namespace Assets.Scripts.Runtime.Events.PlayerSpellCastEvent
{
    public class PlayerSpellCastedEvent : GameEventBase
    {
        public SpellSO Spell { get; private set; }
        public PlayerSpellCastedEvent(SpellSO spellSO)
        {
            Spell = spellSO;
        }
    }
}