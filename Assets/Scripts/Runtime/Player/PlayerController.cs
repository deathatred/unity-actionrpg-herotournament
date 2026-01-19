using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [Inject] private EventBus _eventBus;
    [Inject] private PlayerWallCheck _playerWallCheck;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _rotationSpeed = 10f;

    private float _moveSpeed = 5f;
    private CancellationTokenSource _moveCts;
    private Tween _rotationTween;
    private MoveCommand? _currentCommand;
    public bool IsMoving { get; private set; }

    private void Awake()
    {
        StartMovementLoop();
    }
    private void OnEnable()
    {
        _eventBus.Subscribe<MoveSpeedChangedEvent>(OnMoveSpeedChanged);
        _eventBus.Subscribe<LevelLoadedEvent>(OnLevelLoaded);
    }
    private void OnDisable()
    {
        _eventBus.Unsubscribe<MoveSpeedChangedEvent>(OnMoveSpeedChanged);
        _eventBus.Unsubscribe<LevelLoadedEvent>(OnLevelLoaded);
    }
    private void OnDestroy()
    {
        _moveCts?.Cancel();
        _moveCts?.Dispose();
    }
    public void MoveTo(MoveCommand command)
    {
        _currentCommand = command;
    }
    public void Stop()
    {
        _currentCommand = null;
        IsMoving = false;
    }
    public void WarpToPosition(Vector3 position)
    {
        _rb.MovePosition(position);
    }
    public Rigidbody GetRb()
    {
        return _rb;
    }
    private void OnMoveSpeedChanged(MoveSpeedChangedEvent e)
    {
        _moveSpeed = e.Amount;
    }
    private void OnLevelLoaded(LevelLoadedEvent e)
    {
        Stop();
        print("warped");
        WarpToPosition(e.SpawnPoint.position);
    }
    private void StartMovementLoop()
    {
        _moveCts = new CancellationTokenSource();
        MoveLoopAsync(_moveCts.Token).Forget();
    }
    private async UniTask MoveLoopAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (_currentCommand.HasValue)
            {
                ProcessMovement(_currentCommand.Value);
            }

            await UniTask.WaitForFixedUpdate(token);
        }
    }
    private void ProcessMovement(MoveCommand command)
    {
        if (command.Target == null)
        {
            _currentCommand = null;
            IsMoving = false;
            return;
        }

        Vector3 targetPos = command.Target.GetPosition();

        Vector3 dir = targetPos - _rb.position;
        dir.y = 0f;

        float distance = dir.magnitude;
        IsMoving = distance > command.StopRange;

        if (command.RotateTowardsTarget && dir != Vector3.zero )
        {
            RotateToTarget(dir);
        }

        if (!IsMoving)
        {
            _currentCommand = null;
            return;
        }

        if (!_playerWallCheck.WallCheck(_rb, dir))
        {
            Vector3 step = dir.normalized * _moveSpeed * Time.fixedDeltaTime;

            if (step.magnitude > distance)
            {
                step = dir; 
            }

            _rb.MovePosition(_rb.position + step);
        }
    }
    private void RotateToTarget(Vector3 dir)
    {
        dir.y = 0f;
        if (dir == Vector3.zero) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        Quaternion smoothRot = Quaternion.Slerp(
            _rb.rotation,
            targetRot,
            _rotationSpeed * Time.fixedDeltaTime
        );

        _rb.MoveRotation(smoothRot);
    }

    public async UniTask RotateToTargetAsync(Vector3 targetPos, CancellationToken token)
    {
        Vector3 lookAtPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        float rotationDuration = GlobalData.ROTATION_DURATION;
        _rotationTween?.Kill();

        _rotationTween = transform
            .DOLookAt(lookAtPos, rotationDuration)
            .SetEase(Ease.Linear);

        await _rotationTween
            .AsyncWaitForCompletion()
            .AsUniTask()
            .AttachExternalCancellation(token);
    }
}