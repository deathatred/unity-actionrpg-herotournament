using UnityEngine;

public class SkeletonAnimation : SummonAnimationBase
{
    private static int AttackModifierHash = Animator.StringToHash("AttackModifier");

    public void SetAttackModifier(int modifier)
    {
        _animator.SetInteger(AttackModifierHash, modifier);
    }
}
