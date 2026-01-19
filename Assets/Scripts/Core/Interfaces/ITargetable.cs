using UnityEngine;

public interface ITargetable
{
    public Transform Transform { get;}
    public IHealthSystem HealthSystem { get;}
}
