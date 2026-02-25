using UnityEngine;

namespace Assets.Scripts.Runtime.BaseLogic
{
    public abstract class StateMachineBase<TState> : MonoBehaviour where TState : class, IState
    {
        public TState CurrentState { get; private set; }

        protected virtual void Update()
        {
            CurrentState?.Update();
        }

        public void ChangeState(TState newState)
        {
            if (CurrentState == newState) return;

            CurrentState?.Exit();

            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}