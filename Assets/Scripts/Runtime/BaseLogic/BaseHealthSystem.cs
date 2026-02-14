using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public abstract class BaseHealthSystem<TData> : MonoBehaviour, IHealthSystem where TData : ScriptableObject
{
    [Inject] protected EventBus _eventBus;

    [SerializeField] protected TData _data;
  
    public int Health => _health;
    public int Armor => _armor;
    public bool IsDead => _isDead;
    private int _maxHealth;
    private int _health;
    private int _armor;
    private bool _isDead;

    public event EventHandler OnDeath;
    public event Action<int, int> OnHealthChanged;


    private void Awake()
    {
        _maxHealth = GetMaxHpFromData();
        _health = _maxHealth;
    }
    protected abstract void DeathLogic();
 
    public abstract int GetMaxHpFromData();
    public void Heal(int amount)
    {
        _health = Mathf.Min(_health + amount, _maxHealth);
    }
    public virtual int TakeDamage(int amount)
    {
        int amountAfterArmor = Mathf.Max(amount - _armor, 0);
        _health -= amountAfterArmor;
        if (_health <= 0)
        {
            _isDead = true;
            OnDeath?.Invoke(this, EventArgs.Empty);
            DeathLogic();
        }
        OnHealthChanged.Invoke(_health, _maxHealth);
        return amountAfterArmor;
    } 
    public void SetIsDeadTrue()
    {
        _isDead = true;
    }
    public void RestoreHealth(int amount)
    {
        if (amount > _maxHealth)
        {
            _health = _maxHealth;
        }
        else
        {
            _health = amount;
        }
        OnHealthChanged.Invoke(_health, _maxHealth);
    }
   
}
