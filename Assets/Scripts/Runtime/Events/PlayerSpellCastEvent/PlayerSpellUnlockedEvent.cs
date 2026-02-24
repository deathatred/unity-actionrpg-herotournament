using Assets.Scripts.Core.Observer;
using UnityEngine;

public class PlayerSpellUnlockedEvent : GameEventBase
{
    public SpellSO SpellSO { get; private set; }
    public PlayerSpellUnlockedEvent(SpellSO spellSO)
    {
        SpellSO = spellSO;
    }
}
