using System;
using UnityEngine;
using Zenject;

public class PlayerHealthSystem : MonoBehaviour, IHealthSystem
{
    [Inject] private EventBus _eventBus;
    [Inject] private PlayerStats _playerStats;
    public int Health => _currentHealth;
    public int Armor => _armor;
    public bool IsDead => _isDead;

    private int _currentHealth;
    private int _maxHealth;
    private int _armor;
    private bool _isDead;

    public event Action<int, int> OnHealthChanged;
    public event EventHandler OnDeath;

    private void OnEnable()
    {
        SubscribeToEvents();
    }
    private void Start()
    {
        
    }
    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    private void SubscribeToEvents()
    {
        _eventBus.Subscribe<StatChangedEvent>(StatChanged);
    }
    private void UnsubscribeFromEvents()
    {
        _eventBus.Unsubscribe<StatChangedEvent>(StatChanged);
    }
    private void StatChanged(StatChangedEvent e)
    {
        if (e.StatType == StatType.MaxHealth)
        {
            float oldMax = _maxHealth;
            float oldCurrent = _currentHealth;

            _maxHealth = e.Amount;

            if (oldMax > 0f)
            {
                _currentHealth = (int)((oldCurrent / oldMax) * _maxHealth);
            }
            else
            {
                _currentHealth = _maxHealth;
            }
            _eventBus.Publish(new CurrentHealthChangedEvent(_currentHealth));
        }
        else if (e.StatType == StatType.Armor)
        {
            _armor = e.Amount;
        }
    }
    public void Heal(int amount)
    {  
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        _eventBus.Publish(new PlayerHealthChangedEvent(_maxHealth, _currentHealth));
        _eventBus.Publish(new CurrentHealthChangedEvent(_currentHealth));
    }
    public int TakeDamage(int amount)
    {
        int amountAfterArmor = Mathf.Max(amount - _armor, 0);
        _currentHealth -= amountAfterArmor;
        _eventBus.Publish(new CurrentHealthChangedEvent(_currentHealth));
        _eventBus.Publish(new PlayerHealthChangedEvent(_maxHealth, _currentHealth));
        if (_currentHealth <= 0)
        {
            _isDead = true;
            _eventBus.Publish(new PlayerDeadEvent());
        }
        return amountAfterArmor;
    }
    public void RestoreHealth(int amount)
    {
        _maxHealth = _playerStats.GetStat(StatType.MaxHealth);
        _currentHealth = Mathf.Min(amount, _maxHealth);
        _eventBus.Publish(new PlayerHealthChangedEvent(_maxHealth, _currentHealth));
        _eventBus.Publish(new CurrentHealthChangedEvent(_currentHealth));
    }
}
