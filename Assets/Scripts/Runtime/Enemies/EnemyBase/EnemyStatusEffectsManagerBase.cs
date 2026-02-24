using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class EnemyStatusEffectsManager : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private Renderer[] _renderers;
    private Material[][] _baseMaterials;
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
    [SerializeField] private EnemyControllerBase _controller;
    [SerializeField] private EnemyHealthSystem _healthSystem;
    [SerializeField] private EnemyAnimationBase _enemyAnimationBase;

    [Header("Freeze Settings")]
    [SerializeField] private Material _freezeMaterial;

    private readonly Dictionary<EnemyStatusEffectSO, StatusEffectInstance> _activeEffects = new();
    private readonly List<SlowStatusEffectSO> _activeSlows = new();
    private readonly List<Material> _activeOverlayMaterials = new();


    private void Awake()
    {
        _baseMaterials = new Material[_renderers.Length][];

        for (int i = 0; i < _renderers.Length; i++)
        {
            _baseMaterials[i] = _renderers[i].materials;
        }
    }
    private void OnDisable()
    {
        RemoveDebuffsOnDeath();
    }

    #region Slow management

    public void AddSlowEffect(SlowStatusEffectSO effect)
    {
        if (_activeSlows.Contains(effect))
            return;

        _activeSlows.Add(effect);
        RecalculateSpeed();
    }

    public void RemoveSlowEffect(SlowStatusEffectSO effect)
    {
        if (!_activeSlows.Remove(effect))
            return;

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

    #region Freeze management

    public void ApplyFreeze()
    {
        if (_enemyStateMachine.IsStunned)
        {
            return;
        }

        _controller.SetSpeed(0f);
        _enemyAnimationBase.Freeze();
        _enemyStateMachine.SetIsStunnedTrue();

        ApplyStatusEffectMaterial(_freezeMaterial);
    }

    public void RemoveFreeze()
    {
        if (!_enemyStateMachine.IsStunned)
            return;

        _controller.SetSpeed(_controller.GetBaseSpeed());
        _enemyAnimationBase.Unfreeze();
        _enemyStateMachine.SetIsStunnedFalse();

        RemoveStatusEffectMaterial(_freezeMaterial);
    }

    #endregion

    #region Material overlay logic

    private void ApplyStatusEffectMaterial(Material material)
    {
        if (_activeOverlayMaterials.Contains(material))
            return;

        _activeOverlayMaterials.Add(material);
        RebuildMaterials();
    }

    private void RemoveStatusEffectMaterial(Material material)
    {
        if (!_activeOverlayMaterials.Remove(material))
            return;
        RebuildMaterials();

    }
    private void RebuildMaterials()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            var baseMats = _baseMaterials[i];
            var newMats = new Material[baseMats.Length + _activeOverlayMaterials.Count];

            baseMats.CopyTo(newMats, 0);
            _activeOverlayMaterials.CopyTo(newMats, baseMats.Length);

            _renderers[i].materials = newMats;
        }
    }


    #endregion

    #region Status Effect Runner
    public void ApplyStatusEffect(EnemyStatusEffectSO effect)
    {
        if (_activeEffects.TryGetValue(effect, out var instance))
        {
            instance.Cts.Cancel();
            instance.Cts = new CancellationTokenSource();
            RunEffectTimer(effect, instance.Cts).Forget();
            return;
        }
        var newInstance = new StatusEffectInstance(effect);
        _activeEffects.Add(effect, newInstance);

        effect.Apply(this);
        _enemyStateMachine.GoToAttackState();
        RunEffectTimer(effect, newInstance.Cts).Forget();
    }
    private async UniTask RunEffectTimer(EnemyStatusEffectSO effect, CancellationTokenSource cts)
    {
        try
        {
            await UniTask.WaitForSeconds(effect.Duration).AttachExternalCancellation(cts.Token);
        }
        catch (OperationCanceledException)
        {
            return;
        }
        if (this == null || gameObject == null)
        {
            return;
        }
        effect.Remove(this);
        _activeEffects.Remove(effect);
    }
    private void RemoveDebuffsOnDeath()
    {
        foreach (var effect in _activeEffects)
        {
            effect.Value.Cts.Cancel();
        }
    }
    #endregion

    public EnemyControllerBase GetControllerBase() => _controller;
}
