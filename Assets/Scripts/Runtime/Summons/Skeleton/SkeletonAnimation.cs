using Assets.Scripts.Runtime.Summons.SummonsBase;
using UnityEngine;

namespace Assets.Scripts.Runtime.Summons.Skeleton
{
    public class SkeletonAnimation : SummonAnimationBase
    {
        private static int AttackModifierHash = Animator.StringToHash("AttackModifier");

        public void SetAttackModifier(int modifier)
        {
            _animator.SetInteger(AttackModifierHash, modifier);
        }
    }
}