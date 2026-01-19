using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EyeOfCthulhuController : EnemyControllerBase
{
    [SerializeField] private Transform _visual;
    [SerializeField] private EnemyHealthSystem _healthSystem;
    private float _hoverHeight = 0.1f;
    private float _hoverAmplitude = 0.2f;
    private float _hoverSpeed = 2f;
    private float _baseY = -1f;
    private void Update()
    {
        if (_healthSystem.IsDead) return;
        float hoverOffset = Mathf.Sin(Time.time * _hoverSpeed) * _hoverAmplitude;
        Vector3 pos = transform.position;
        pos.y = _baseY + _hoverHeight + hoverOffset;
        _visual.transform.position = pos;
    } 
}
