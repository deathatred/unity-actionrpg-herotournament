using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusEffectsManager : MonoBehaviour
{
    [SerializeField] private EnemyControllerBase _controller;
    [SerializeField] private EnemyHealthSystem _healthSystem;

    private readonly List<SlowStatusEffectSO> _activeSlows = new();

    public async UniTask ApplyStatusEffect(EnemyStatusEffectSO effect)
    {
        effect.Apply(this); 
        await UniTask.WaitForSeconds(effect.Duration);
        effect.Remove(this); 
    }

    #region Slow management

    public void AddSlowEffect(SlowStatusEffectSO effect)
    {
        _activeSlows.Add(effect);
        RecalculateSpeed();
    }

    public void RemoveSlowEffect(SlowStatusEffectSO effect)
    {
        _activeSlows.Remove(effect);
        RecalculateSpeed();
    }


    private void RecalculateSpeed()
    {
        float totalSlowPercent = 0f;
        foreach (var slow in _activeSlows)
            totalSlowPercent += slow.slowPercent;

        totalSlowPercent = Mathf.Clamp(totalSlowPercent, 0f, 100f);

        float multiplier = 1f - (totalSlowPercent / 100f);

        _controller.SetSpeed(_controller.GetBaseSpeed() * multiplier);
    }


    #endregion

    public EnemyControllerBase GetControllerBase() => _controller;
}
