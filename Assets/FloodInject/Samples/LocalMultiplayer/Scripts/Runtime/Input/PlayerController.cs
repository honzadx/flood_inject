using System.Collections;
using UnityEngine.InputSystem.DualShock;
using FloodInject.Runtime;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LocalMultiplayer.Runtime
{
    public class PlayerController : MonoBehaviour
    {
        private static int _playerCount;
        
        [SerializeField] private int _playerIndex;
        [SerializeField] private PlayerInput _playerInput;

        private PlayerInputRelay _playerInputRelay;
        private InputDevice _inputDevice;
        private bool _initialized;

        public int PlayerIndex => _playerIndex;
        
        private void Init()
        {
            _playerIndex = _playerCount++;
            _playerInputRelay = ContextProvider<GameContext>.Ctx
                .Get<GameInputManager>()
                .GetInputRelay(_playerIndex);
            _initialized = true;
            
            RefreshInputDevice();
        }
        
        protected void Start()
        {
            if (!_initialized) Init();
        }

        protected void OnMovement(InputValue value)
        {
            if (!_initialized) Init();
            _playerInputRelay.HandleInput(PlayerActionType.Movement, value);
        }
        
        protected void OnAction1(InputValue value)
        {
            if (!_initialized) Init();
            _playerInputRelay.HandleInput(PlayerActionType.Action1, value);
        }

        protected void OnAction2(InputValue value)
        {
            if (!_initialized) Init();
            _playerInputRelay.HandleInput(PlayerActionType.Action2, value);
        }
        
        [UsedImplicitly] // Called implicitly by PlayerInput
        protected IEnumerator OnDeviceLost()
        {
            // Wait a single frame for the internal data to update
            yield return null;
            RefreshInputDevice();
            Debug.Log($"Player[{_playerIndex}] OnDeviceLost");
        }
        
        [UsedImplicitly] // Called implicitly by PlayerInput
        protected IEnumerator OnDeviceRegained()
        {
            // Wait a single frame for the internal data to update
            yield return null;
            RefreshInputDevice();
            Debug.Log($"Player[{_playerIndex}] OnDeviceRegained");
        }

        protected void RefreshInputDevice()
        {
            if (_playerInput.devices.Count == 0)
            {
                _playerInputRelay.SetInputDevice(InputDevice.None);
                return;
            }
            
            var inputDevice = _playerInput.currentControlScheme switch
            {
                "Keyboard" => InputDevice.Keyboard,
                "Gamepad" => InputDevice.Gamepad,
                _ => InputDevice.None
            };
            if (inputDevice == InputDevice.Gamepad)
            {
                if (_playerInput.GetDevice<Gamepad>() is DualShockGamepad dualShockGamepad)
                {
                    var gameInit = ContextProvider<GameContext>.Ctx.Get<GameInitSO>();
                    var playerColor = gameInit.PlayerColors[_playerIndex];
                    dualShockGamepad.SetLightBarColor(playerColor);
                }
            }
            _playerInputRelay.SetInputDevice(inputDevice);
        }
    }
}