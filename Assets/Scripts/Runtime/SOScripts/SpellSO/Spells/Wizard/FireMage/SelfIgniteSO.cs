using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "SelfIgnite", menuName = "Wizard Spells/SelfIgnite")]
public class SelfIgniteSO : SpellSO
{
    public int SpellPowerBonus;
    public float BonusDuration;
    public override void Activate(PlayerSpellContext ctx)
    {
        ctx.Audio.PlaySpellAudio(this);
        ctx.Stats.ApplyTemporaryBonusAsync(GlobalData.SELF_IGNITE_EFFECT,StatType.SpellPower, SpellPowerBonus, BonusDuration).Forget();
    }
}
