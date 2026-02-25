using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Player.FSM;
using UnityEngine;

namespace Assets.Scripts.Core.Interfaces
{
    public abstract class PlayerStateBase
    {
        protected PlayerStateMachine _fsm;
        public PlayerState StateType { get; protected set; }

        public PlayerStateBase(PlayerStateMachine fsm)
        {
            _fsm = fsm;
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}