using System;
using UnityEngine;

public class GolemAnimation : EnemyAnimationBase
{
    private static int SmashHash = Animator.StringToHash("Smash");
    private static int AttackModifierHash = Animator.StringToHash("AttackModifier");
    //public void TriggerSmash()
    //{
    //    _animator.SetTrigger(SmashHash);
    //}
    public void SetSmashIsTrue()
    {
        _animator.SetBool(SmashHash, true);
    }
    public void SetSmashIsFalse()
    {
        _animator.SetBool(SmashHash, false);
    }
    protected override void HealthSystemOnDeath(object sender, EventArgs e)
    {
        base.HealthSystemOnDeath(sender, e);
    }
    public override void TriggerAttack()
    {
        int random = UnityEngine.Random.Range(0, 2);
        _animator.SetInteger(AttackModifierHash, random);
        base.TriggerAttack();
    }
}
