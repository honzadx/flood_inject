using System;
using FloodInject.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LocalMultiplayer.Runtime
{
    public class PlayerController : MonoBehaviour
    {
        private static int _playerCount;
        
        [SerializeField] private int _playerIndex;
        [SerializeField] private PlayerInput _playerInput;

        private InputDevice _inputDevice;
        private bool _initialized;
        private BaseContext _playerContext;

        public void Start()
        {
            if (!_initialized) Init();
        }

        private void Init()
        {
            _playerCount++;
            _playerIndex = _playerCount;
            _playerContext = Runtime.PlayerContext.GetPlayerContextFromIndex(_playerIndex);
            _playerContext.Bind(this);
            var inputDevice = _playerInput.currentControlScheme switch
            {
                "keyboard" => InputDevice.Keyboard,
                "gamepad" => InputDevice.Gamepad,
                _ => InputDevice.Keyboard
            };
            _playerContext.Get<PlayerInputRelay>().SetInputDevice(inputDevice);
            _initialized = true;
        }

        public int PlayerIndex => _playerIndex;

        protected void OnMovement(InputValue value)
        {
            if (!_initialized) Init();
            _playerContext.Get<PlayerInputRelay>()?.HandleInput(PlayerInputActionType.Movement, value);
        }
        
        protected void OnAction1(InputValue value)
        {
            if (!_initialized) Init();
            _playerContext.Get<PlayerInputRelay>()?.HandleInput(PlayerInputActionType.Action1, value);
        }

        protected void OnAction2(InputValue value)
        {
            if (!_initialized) Init();
            _playerContext.Get<PlayerInputRelay>()?.HandleInput(PlayerInputActionType.Action2, value);
        }
        
        protected void OnDeviceLost()
        {
            Debug.Log($"Player[{_playerIndex}] OnDeviceLost");
        }

        protected void OnDeviceRegained()
        {
            var inputDevice = _playerInput.currentControlScheme switch
            {
                "keyboard" => InputDevice.Keyboard,
                "gamepad" => InputDevice.Gamepad,
                _ => throw new ArgumentOutOfRangeException()
            };
            _playerContext.Get<PlayerInputRelay>().SetInputDevice(inputDevice);
            Debug.Log($"Player[{_playerIndex}] OnDeviceRegained");
        }
    }
}