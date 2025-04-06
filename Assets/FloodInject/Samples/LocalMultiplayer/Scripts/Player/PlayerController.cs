using System;
using FloodInject.Runtime;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

[Flood]
public partial class PlayerController : MonoBehaviour
{
    public event Action DeviceLostEvent;
    public event Action DeviceRegainedEvent;
    
    public event Action<Vector2> MoveEvent;
    public event Action Action1Event;
    public event Action Action2Event;
    
    public int PlayerIndex { get; private set; }
    public Vector2 MovementDirection { get; private set; }

    [Resolve(typeof(PlayerContext))] PlayerManager _playerManager;

    public InputActionReference moveAction;
    public InputActionReference input1Action;
    public InputActionReference input2Action;

    protected void Start()
    {
        Construct();
        moveAction.action.performed += OnMove;
        input1Action.action.performed += OnAction1;
        input2Action.action.performed += OnAction2;
        int index = 0;
        // Assign to first available controller
        foreach (var playerController in _playerManager.Controllers)
        {
            if (playerController == null)
            {
                _playerManager.AssignController(this, PlayerIndex = index);
                break;
            }
            index++;
        }
    }
    
    protected void Update()
    {
        MovementDirection = moveAction.action.ReadValue<Vector2>();
    }
    
    private void OnMove(InputAction.CallbackContext context)
    {
        MovementDirection = context.action.ReadValue<Vector2>();
        MoveEvent?.Invoke(MovementDirection);
    }
    
    private void OnAction1(InputAction.CallbackContext context)
    {
        Action1Event?.Invoke();
    }
    
    private void OnAction2(InputAction.CallbackContext context)
    {
        Action2Event?.Invoke();
    }
    
    [UsedImplicitly] // Callback from PlayerInput
    private void OnDeviceLost()
    {
        DeviceLostEvent?.Invoke();
    }
    
    [UsedImplicitly] // Callback from PlayerInput
    private void OnDeviceRegained()
    {
        DeviceRegainedEvent?.Invoke();
    }
}