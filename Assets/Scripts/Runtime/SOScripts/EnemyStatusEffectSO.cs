using Cysharp.Threading.Tasks;
using UnityEngine;


public abstract class EnemyStatusEffectSO : ScriptableObject
{
    public float Duration;

    public abstract void Apply(EnemyStatusEffectsManager target);
    public abstract void Remove(EnemyStatusEffectsManager target);
}
