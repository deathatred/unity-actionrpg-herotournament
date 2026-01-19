using UnityEngine;

[CreateAssetMenu(fileName = "DoubleSlash", menuName = "Player Spells/DoubleSlash")]
public class DoubleSlashSO : SpellSO
{
    public override void Activate(PlayerSpellContext ctx)
    {
        if (ctx.EnemyHealthSystem == null)
        {
            return;
        }
        ctx.EnemyHealthSystem.TakeDamage(Mathf.RoundToInt(ctx.Stats.GetStat(StatType.AttackPower) * 1.5f));
        ctx.Audio.PlaySpellAudio(this);
    }
}
