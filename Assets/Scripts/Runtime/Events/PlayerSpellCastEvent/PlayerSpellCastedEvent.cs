using Assets.Scripts.Core.Observer;
using UnityEngine;

public class PlayerSpellCastedEvent : GameEventBase
{
    public SpellSO Spell {  get; private set; }
    public PlayerSpellCastedEvent(SpellSO spellSO)
    {
        Spell = spellSO;
    }
}
