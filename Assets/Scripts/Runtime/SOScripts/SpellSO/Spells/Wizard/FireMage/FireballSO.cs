using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Fireball", menuName = "Wizard Spells/Fireball")]
public class FireballSO : SpellSO
{
    public ProjectileSO FireballProjectile;
    public override void Activate(PlayerSpellContext ctx)
    {
        ctx.SpellCasting.ExecuteProjectileSpellAsync(FireballProjectile, ctx.EnemyTransform).Forget();
        ctx.Audio.PlaySpellAudio(this);
    }
}
