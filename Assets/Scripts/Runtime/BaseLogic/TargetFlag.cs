using Assets.Scripts.Core.Interfaces;
using UnityEngine;

public class TargetFlag : MonoBehaviour, ITargetable
{
    [SerializeField] Transform _targetTransform;
    private IHealthSystem _healthSystem;
    public Transform Transform => _targetTransform;
    public IHealthSystem HealthSystem => _healthSystem;

    private void Awake()
    {
        _healthSystem = GetComponent<IHealthSystem>();
    }
}
   
