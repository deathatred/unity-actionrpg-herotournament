using Assets.Scripts.Core.Interfaces;
using Assets.Scripts.Runtime.Enums;
using Assets.Scripts.Runtime.Player.FSM;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.Factories
{
    public class PlayerStateFactory
    {
        private readonly DiContainer _container;
        public PlayerStateFactory(DiContainer container)
        {
            _container = container;
        }
        public T CreateState<T>() where T : PlayerStateBase
        {
            return _container.Instantiate<T>();
        }
        public PlayerStateBase CreatePlayerState(PlayerState playerState)
        {
            switch (playerState)
            {
                case PlayerState.Idle:
                    return CreateState<PlayerIdleState>();
                case PlayerState.Moving:
                    return CreateState<PlayerMovingState>();
                case PlayerState.Attacking:
                    return CreateState<PlayerAttackState>();
                case PlayerState.Collecting:
                    return CreateState<PlayerCollectingState>();
                case PlayerState.SpellCasting:
                    return CreateState<PlayerSpellCastingState>();
                case PlayerState.MoveToSpellTarget:
                    return CreateState<PlayerMoveToSpellTargetState>();
                case PlayerState.Interacting:
                    return CreateState<PlayerInteractingState>();
            }
            Debug.LogError("State does not exist");
            return null;
        }
    }
}