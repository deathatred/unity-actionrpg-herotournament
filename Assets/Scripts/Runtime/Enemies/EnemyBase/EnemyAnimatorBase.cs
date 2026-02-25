using Assets.Scripts.Runtime.BaseLogic;
using UnityEngine;

namespace Assets.Scripts.Runtime.Enemies.EnemyBase
{
    public class EnemyAnimationBase : CharacterAnimationBase<EnemyHealthSystem>
    {
        protected override void SubscribeToDeath()
        {
            _healthSystem.OnDeath += HealthSystemOnDeath;
        }

        protected virtual void HealthSystemOnDeath(object sender, System.EventArgs e)
        {
            SetIsDeadTrue();
        }

        protected override void UnsubscribeFromDeath()
        {
            _healthSystem.OnDeath -= HealthSystemOnDeath;
        }
    }
}