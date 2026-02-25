using Assets.Scripts.Runtime.Enemies.EnemyBase;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Runtime.SOScripts
{
    public abstract class EnemyStatusEffectSO : ScriptableObject
    {
        public float Duration;

        public abstract void Apply(EnemyStatusEffectsManager target);
        public abstract void Remove(EnemyStatusEffectsManager target);
    }
}