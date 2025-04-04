using System;
using FloodInject.Runtime;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

[Flood]
public partial class PlayerController : MonoBehaviour
{
    public event Action OnDeviceLostEvent;
    public event Action OnDeviceRegainedEvent;
    
    public event Action<Vector2> OnMoveEvent;
    public event Action OnAction1Event;
    public event Action OnAction2Event;
    
    public int PlayerIndex { get; private set; }
    
    [Resolve(typeof(PlayerContext))] PlayerManager _playerManager;

    protected void Start()
    {
        Construct();
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
    
    private void OnMove(InputValue value)
    {
        OnMoveEvent?.Invoke(value.Get<Vector2>());
    }
    
    private void OnAction1(InputValue value)
    {
        OnAction1Event?.Invoke();
    }

    private void OnAction2(InputValue value)
    {
        OnAction2Event?.Invoke();
    }
    
    [UsedImplicitly] // Callback from PlayerInput
    private void OnDeviceLost()
    {
        OnDeviceLostEvent?.Invoke();
    }
    
    [UsedImplicitly] // Callback from PlayerInput
    private void OnDeviceRegained()
    {
        OnDeviceRegainedEvent?.Invoke();
    }
}