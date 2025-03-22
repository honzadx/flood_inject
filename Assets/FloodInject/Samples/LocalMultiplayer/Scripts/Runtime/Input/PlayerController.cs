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
        
        protected void OnDeviceLost()
        {
            Debug.Log($"Player[{_playerIndex}] OnDeviceLost");
        }

        protected void OnDeviceRegained()
        {
            RefreshInputDevice();
            Debug.Log($"Player[{_playerIndex}] OnDeviceRegained");
        }

        protected void RefreshInputDevice()
        {
            var inputDevice = _playerInput.currentControlScheme switch
            {
                "keyboard" => InputDevice.Keyboard,
                "gamepad" => InputDevice.Gamepad,
                _ => InputDevice.Keyboard
            };
            _playerInputRelay.SetInputDevice(inputDevice);
        }
    }
}