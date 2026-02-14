using UnityEngine;

[CreateAssetMenu(fileName = "FreezeStatusEffectSO", menuName = "EnemyStatusEffects/Freeze")]
public class FreezeStatusEffectSO : EnemyStatusEffectSO
{
    public override void Apply(EnemyStatusEffectsManager target)
    {
        target.ApplyFreeze();
    }

    public override void Remove(EnemyStatusEffectsManager target)
    {
        target.RemoveFreeze();
    }
}
