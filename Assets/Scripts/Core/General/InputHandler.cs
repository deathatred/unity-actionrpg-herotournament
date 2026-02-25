using UnityEngine;
using static PlayerInput;

namespace Assets.Scripts.Core.General
{
    public static class InputHandler
    {
        private static PlayerInput _playerInput = new PlayerInput();

        public static void EnablePlayerInput()
        {
            _playerInput.Enable();
        }
        public static void DisablePlayerInput()
        {
            _playerInput.Disable();
        }
        public static void SubscribeToPlayerInput(IPlayerActions obj)
        {
            _playerInput.Player.SetCallbacks(obj);
        }
    }
}