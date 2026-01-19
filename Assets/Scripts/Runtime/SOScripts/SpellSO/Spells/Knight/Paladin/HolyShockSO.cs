using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "HolyShock", menuName = "Player Spells/HolyShock")]
public class HolyShockSO : SpellSO
{
    public ProjectileSO LightProjectile;
    public override void Activate(PlayerSpellContext ctx)
    {
        ctx.SpellCasting.ExecuteProjectileSpellAsync(LightProjectile,
            ctx.EnemyTransform).Forget();
        ctx.Audio.PlaySpellAudio(this);
    }
}
