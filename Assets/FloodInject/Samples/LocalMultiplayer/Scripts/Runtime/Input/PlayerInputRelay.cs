using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LocalMultiplayer.Runtime
{
    /// <summary>
    /// Each playerIndex has a matching relay that always exist in the context of the application. 
    /// </summary>
    public class PlayerInputRelay
    {
        public event Action<InputDevice> InputDeviceChangedEvent;
        public event Action<Vector2> MoveEvent;
        public event Action<bool> Action1Event;
        public event Action<bool> Action2Event;
        
        public readonly int PlayerIndex;
        
        public InputDevice InputDevice { get; private set; }

        public PlayerInputRelay(int playerIndex)
        {
            this.PlayerIndex = playerIndex;
        }

        public void HandleInput(PlayerActionType actionType, InputValue inputValue)
        {
            switch (actionType)
            {
                case PlayerActionType.Movement:
                    MoveEvent?.Invoke(inputValue.Get<Vector2>());
                    break;
                case PlayerActionType.Action1:
                    Action1Event?.Invoke(inputValue.isPressed);
                    break;
                case PlayerActionType.Action2:
                    Action2Event?.Invoke(inputValue.isPressed);
                    break;
            }
        }

        public void SetInputDevice(InputDevice inputDevice)
        {
            InputDevice = inputDevice;
            InputDeviceChangedEvent?.Invoke(inputDevice);
        }
    }
}