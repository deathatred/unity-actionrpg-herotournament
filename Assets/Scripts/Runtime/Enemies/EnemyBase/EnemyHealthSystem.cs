using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class EnemyHealthSystem : BaseHealthSystem<EnemyDataSO>, IHealthSystem
{
    [SerializeField] private Collider[] _colliders;
    [SerializeField] private Image _targetImage;
    public override int GetMaxHpFromData()
    {
        return _data.MaxHealth;
    }
    protected override void DeathLogic()
    {
        foreach (var collider in _colliders)
        {
            collider.enabled = false;
        }
        _eventBus.Publish(new EnemyKilledEvent(_data.XpReward,gameObject));
    }
    public void SetTarget()
    {
        _targetImage.gameObject.SetActive(true);
    }
    public void UnsetTarget()
    {
        _targetImage.gameObject.SetActive(false);
    }
}
