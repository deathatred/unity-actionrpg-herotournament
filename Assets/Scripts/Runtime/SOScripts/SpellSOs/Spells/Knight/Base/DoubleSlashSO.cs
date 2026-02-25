using Assets.Scripts.Core.Enums;
using Assets.Scripts.Runtime.SOScripts.SpellSOs;
using Assets.Scripts.Runtime.SpellsContext;
using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts.SpellSOs.Spells
{
    [CreateAssetMenu(fileName = "DoubleSlash", menuName = "Player Spells/DoubleSlash")]
    public class DoubleSlashSO : SpellSO
    {
        public override void Activate(PlayerSpellContext ctx)
        {
            if (ctx.EnemyHealthSystem == null)
            {
                return;
            }
            ctx.EnemyHealthSystem.TakeDamage(Mathf.RoundToInt(ctx.Stats.GetBaseStat(StatType.AttackPower) * 1.5f));
            ctx.Audio.PlaySpellAudio(this);
        }
    }
}