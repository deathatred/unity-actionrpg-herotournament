using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class EnemyHealthSystem : BaseHealthSystem<EnemyDataSO>, IHealthSystem
{
    [SerializeField] private Collider[] _colliders;
    [SerializeField] private Image _targetImage;
    [SerializeField] private EnemyStateMachine _enemyStateMachine;
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
    public override int TakeDamage(int amount)
    {
        _enemyStateMachine.GoToAttackState();
        return base.TakeDamage(amount);
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
