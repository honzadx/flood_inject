using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LocalMultiplayer.Runtime
{
    public class PlayerInputRelay
    {
        public event Action<InputDevice> InputDeviceChangedEvent;
        public event Action<Vector2> MoveEvent;
        public event Action Action1Event;
        public event Action Action2Event;
        
        public readonly int playerIndex;

        private InputDevice _inputDevice;
        
        public InputDevice InputDevice => _inputDevice;

        public PlayerInputRelay(int playerIndex)
        {
            this.playerIndex = playerIndex;
        }

        public void HandleInput(PlayerInputActionType actionType, InputValue inputValue)
        {
            switch (actionType)
            {
                case PlayerInputActionType.Movement:
                    MoveEvent?.Invoke(inputValue.Get<Vector2>());
                    break;
                case PlayerInputActionType.Action1:
                    Action1Event?.Invoke();
                    break;
                case PlayerInputActionType.Action2:
                    Action2Event?.Invoke();
                    break;
            }
        }

        public void SetInputDevice(InputDevice inputDevice)
        {
            _inputDevice = inputDevice;
            InputDeviceChangedEvent?.Invoke(inputDevice);
        }
    }
}