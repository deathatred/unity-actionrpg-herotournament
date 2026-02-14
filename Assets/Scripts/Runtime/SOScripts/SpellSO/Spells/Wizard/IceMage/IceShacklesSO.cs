using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "IceShackles", menuName = "Wizard Spells/IceShackles")]
public class IceShacklesSO : SpellSO
{
    public EnemyStatusEffectSO _statusEffect;
    public override void Activate(PlayerSpellContext ctx)
    {
        ctx.Audio.PlaySpellAudio(this, ctx.EnemyTransform);
        ctx.EnemyStatusEffectsManager.ApplyStatusEffect(_statusEffect);
    }
}
