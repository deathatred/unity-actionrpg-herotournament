using Assets.Scripts.Runtime.Enemies.EnemyBase;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts.EnemyStatusEffectsSO
{
    [CreateAssetMenu(fileName = "SlowStatusEffectSO", menuName = "EnemyStatusEffects/Slow")]
    public class SlowStatusEffectSO : EnemyStatusEffectSO
    {
        public int slowPercent;
        public override void Apply(EnemyStatusEffectsManager target)
        {
            target.AddSlowEffect(this);
        }
        public override void Remove(EnemyStatusEffectsManager target)
        {
            target.RemoveSlowEffect(this);
        }
    }
}