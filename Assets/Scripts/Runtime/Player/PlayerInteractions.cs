using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Core.Observer;
using UnityEngine;
using Zenject;

public class PlayerInteractions : MonoBehaviour
{
    [Inject] private EventBus _eventBus;

    [Header("Layer Settings")]
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private LayerMask _itemsLayerMask;
    [SerializeField] private LayerMask _interactablesLayerMask;

    private float _rayDistance = 100f;
    private Camera _mainCamera;

    private EnemyHealthSystem _currentEnemyTarget;
    private InWorldItem _currentItemTarget;
    private IInteractable _currentInteractableTarget;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    private void HandleEnemyDeath(object sender, System.EventArgs e) => ResetEnemyTarget();
    private bool TryGetHitComponent<T>(Vector3 pointer, LayerMask mask, out T component)
    {
        Ray ray = _mainCamera.ScreenPointToRay(pointer);
        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, mask))
        {
            return hit.collider.TryGetComponent(out component);
        }
        component = default;
        return false;
    }
    private void ClearCurrentEnemy()
    {
        if (_currentEnemyTarget == null) return;

        _currentEnemyTarget.OnDeath -= HandleEnemyDeath;
        _currentEnemyTarget.UnsetTarget();
        _currentEnemyTarget = null;
    }
    private void ClearAllTargets()
    {
        ClearCurrentEnemy();
        _currentItemTarget = null;
        _currentInteractableTarget = null;
    }
    public bool HandleEnemyInteraction(Vector3 pointer)
    {
        if (TryGetHitComponent(pointer, _enemyLayerMask, out EnemyHealthSystem enemy))
        {
            if (_currentEnemyTarget == enemy)
            {
                _eventBus.Publish(new SameEnemyClickedEvent());
                return true;
            }
            ClearAllTargets();
            _currentEnemyTarget = enemy;
            _currentEnemyTarget.SetTarget();
            _currentEnemyTarget.OnDeath += HandleEnemyDeath;
            _eventBus.Publish(new TargetChangedEvent());
            return true;
        }

        if (_currentEnemyTarget != null) ResetEnemyTarget();
        return false;
    }

    public bool HandleItemInteraction(Vector3 pointer)
    {
        if (TryGetHitComponent(pointer, _itemsLayerMask, out InWorldItem item))
        {
            if (_currentItemTarget != item)
            {
                ClearAllTargets();
                _currentItemTarget = item;
                _eventBus.Publish(new TargetChangedEvent());
            }
            return true;
        }
        return false;
    }

    public bool HandleInteractableInteraction(Vector3 pointer)
    {
        if (TryGetHitComponent(pointer, _interactablesLayerMask, out IInteractable interactable))
        {
            if (_currentInteractableTarget != interactable)
            {
                ClearAllTargets();
                _currentInteractableTarget = interactable;
                _eventBus.Publish(new TargetChangedEvent());
            }
            return true;
        }
        return false;
    }

    public bool IsTargetingGround(Vector3 pointer)
    {
        LayerMask forbiddenMask = _enemyLayerMask | _itemsLayerMask | _interactablesLayerMask;
        bool isGround = !Physics.Raycast(_mainCamera.ScreenPointToRay(pointer), _rayDistance, forbiddenMask);
        if (isGround)
        {
            ClearAllTargets();
        }
        return isGround;
    }

    public void ResetEnemyTarget()
    {
        ClearCurrentEnemy();
        _eventBus.Publish(new TargetChangedEvent());
    }
  
    public EnemyHealthSystem GetCurrentEnemyTarget() => _currentEnemyTarget;
    public InWorldItem GetCurrentItemTarget() => _currentItemTarget;
    public IInteractable GetCurrentInteractableTarget() => _currentInteractableTarget;
    public bool HasEnemyTarget() => _currentEnemyTarget != null;
    public bool HasItemTarget() => _currentItemTarget != null;
    public bool HasInteractableTarget() => _currentInteractableTarget != null;
}