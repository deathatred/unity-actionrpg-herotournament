using System;
using UnityEngine;

public class SummonAnimationBase : CharacterAnimationBase<SummonHealthSystem>
{
    protected override void SubscribeToDeath()
    {
        _healthSystem.OnDeath += HealthSystemOnDeath;
    }

    protected virtual void HealthSystemOnDeath(object sender, EventArgs e)
    {
        SetIsDeadTrue();
    }

    protected override void UnsubscribeFromDeath()
    {
        
    }
}
