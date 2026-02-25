using Assets.Scripts.Runtime.SOScripts.SpellSOs;
using Assets.Scripts.Runtime.SpellsContext;
using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts.SpellSOs.Spells
{
    [CreateAssetMenu(fileName = "HolyHeal", menuName = "Player Spells/HolyHeal")]
    public class HolyHealSO : SpellSO
    {
        public int healAmount;
        public override void Activate(PlayerSpellContext ctx)
        {
            ctx.Self.Heal(healAmount);
            ctx.Audio.PlaySpellAudio(this);
        }
    }
}