using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "IceArrow", menuName = "Wizard Spells/IceArrow")]
public class IceArrowSO : SpellSO
{
    public ProjectileSO IceArrow;
    public override void Activate(PlayerSpellContext ctx)
    {
        ctx.SpellCasting.ExecuteProjectileSpellAsync(IceArrow, ctx.EnemyTransform).Forget();
        ctx.Audio.PlaySpellAudio(this);
    }
}
