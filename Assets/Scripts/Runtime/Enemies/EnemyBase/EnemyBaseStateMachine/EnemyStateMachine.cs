using UnityEngine;

public abstract class EnemyStateMachine : StateMachineBase<IEnemyState>
{
    public bool IsStunned { get; private set; }
    protected override void Update()
    {
        if (IsStunned) { return; }
        base.Update();
    }
    private void OnDisable()
    {
        CurrentState.Exit();
    }
    public void SetIsStunnedTrue()
    {
        IsStunned = true;
    }
    public void SetIsStunnedFalse()
    {
        IsStunned = false;
    }
    public abstract void GoToAttackState();
}
