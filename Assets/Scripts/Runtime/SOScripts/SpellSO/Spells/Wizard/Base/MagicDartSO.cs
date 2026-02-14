using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicDart", menuName = "Wizard Spells/MagicDart")]
public class MagicDartSO : SpellSO
{
    public ProjectileSO MagicDart;
    public override void Activate(PlayerSpellContext ctx)
    {
        ctx.SpellCasting.ExecuteProjectileSpellAsync(MagicDart, ctx.EnemyTransform).Forget();
        ctx.Audio.PlaySpellAudio(this);
    }
}