using Assets.Scripts.Core.Enums;
using Assets.Scripts.Runtime.SOScripts.SpellSOs;
using Assets.Scripts.Runtime.SpellsContext;
using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts.SpellSOs.Spells
{
    [CreateAssetMenu(fileName = "VampiricStrike", menuName = "Player Spells/VampiricStrike")]
    public class VampiricStrikeSO : SpellSO
    {
        public override void Activate(PlayerSpellContext ctx)
        {
            int halfOfAtkPwr = Mathf.RoundToInt(ctx.Stats.GetBaseStat(StatType.AttackPower) / 2);
            if (ctx.EnemyHealthSystem == null)
            {
                Debug.LogWarning("Vampiric strike tried to damage null enemy, this should not trigger!");
                return;
            }
            ctx.EnemyHealthSystem.TakeDamage(ctx.Stats.GetBaseStat(StatType.AttackPower) + halfOfAtkPwr);
            ctx.Audio.PlaySpellAudio(this);
            ctx.Audio.PlayAttackAudio();
            ctx.Self.Heal(halfOfAtkPwr);

        }
    }
}