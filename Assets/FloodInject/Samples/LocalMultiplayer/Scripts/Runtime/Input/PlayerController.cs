using System.Collections;
using UnityEngine.InputSystem.DualShock;
using FloodInject.Runtime;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LocalMultiplayer.Runtime
{
    /// <summary>
    /// Each connected input device will spawn a new instance of this class.
    /// It captures input from the device and broadcasts it through relay.
    /// </summary>
    public sealed class PlayerController : MonoBehaviour
    {
        private static int _playerCount;
        
        [SerializeField] private int _playerIndex;
        [SerializeField] private PlayerInput _playerInput;

        private PlayerInputRelay _playerInputRelay;
        private InputDevice _inputDevice;
        private bool _wasAction1Pressed;
        private bool _wasAction2Pressed;
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
        
        private void Start()
        {
            if (!_initialized) Init();
        }

        private void OnMovement(InputValue value)
        {
            if (!_initialized) Init();
            _playerInputRelay.HandleInput(PlayerActionType.Movement, value);
        }
        
        private void OnAction1(InputValue value)
        {
            if (!_initialized) Init();
            if (value.isPressed == _wasAction1Pressed) return;
            _playerInputRelay.HandleInput(PlayerActionType.Action1, value);
            _wasAction1Pressed = value.isPressed;
        }

        private void OnAction2(InputValue value)
        {
            if (!_initialized) Init();
            if (value.isPressed == _wasAction2Pressed) return;
            _playerInputRelay.HandleInput(PlayerActionType.Action2, value);
            _wasAction2Pressed = value.isPressed;
        }
        
        [UsedImplicitly] // Called implicitly by PlayerInput
        private IEnumerator OnDeviceLost()
        {
            // Wait a single frame for the internal data to update
            yield return null;
            RefreshInputDevice();
            Debug.Log($"Player[{_playerIndex}] OnDeviceLost");
        }
        
        [UsedImplicitly] // Called implicitly by PlayerInput
        private IEnumerator OnDeviceRegained()
        {
            // Wait a single frame for the internal data to update
            yield return null;
            RefreshInputDevice();
            Debug.Log($"Player[{_playerIndex}] OnDeviceRegained");
        }

        private void RefreshInputDevice()
        {
            if (_playerInput.devices.Count == 0)
            {
                _playerInputRelay.SetInputDevice(InputDevice.None);
                return;
            }

            var inputDevice = Statics.ControlSchemeToInputDevice[_playerInput.currentControlScheme];
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