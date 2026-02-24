using Assets.Scripts.Core.Interfaces;
using UnityEngine;

public abstract class CharacterAnimationBase<THealth> : MonoBehaviour
    where THealth : IHealthSystem
{
    [SerializeField] protected Animator _animator;
    [SerializeField] protected THealth _healthSystem;

    protected static readonly int AttackHash = Animator.StringToHash("Attack");
    protected static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    protected static readonly int IsDeadHash = Animator.StringToHash("IsDead");

    protected virtual void OnEnable()
    {
        SubscribeToDeath();
    }

    protected virtual void OnDisable()
    {
        UnsubscribeFromDeath();
    }

    protected abstract void SubscribeToDeath();
    protected abstract void UnsubscribeFromDeath();

    protected virtual void OnDeath()
    {
        SetIsDeadTrue();
    }
    public void Freeze()
    {
        _animator.speed = 0f;
    }
    public void Unfreeze()
    {
        _animator.speed = 1f;
    }
    public void SetIsMovingTrue()
    {
        _animator.SetBool(IsMovingHash, true);
    }
    public void SetIsMovingFalse()
    {
        _animator.SetBool(IsMovingHash, false);
    }

    public void SetIsDeadTrue()
    {
        _animator.SetBool(IsDeadHash, true);
    }

    public virtual void TriggerAttack()
    {
        _animator.SetTrigger(AttackHash);
    }
}

