using System;
using FloodInject.Runtime;
using UnityEngine;
using UnityEngine.Assertions;

[DefaultExecutionOrder(-1)]
public class PlayerManager : MonoBehaviour
{
    public event Action<int, PlayerController> ControllerStateChangedEvent; 
    
    public PlayerController[] Controllers { get; } = new PlayerController[Constants.MAX_PLAYER_COUNT];
    public int ActiveControllerCount { get; private set; }
    
    protected void Start()
    {
        ContextProvider<PlayerContext>.Get().Bind(this);
    }
    
    public void AssignController(PlayerController controller, int playerIndex)
    {
        Controllers[playerIndex] = controller;
        ActiveControllerCount++;
        Assert.IsTrue(ActiveControllerCount <= Constants.MAX_PLAYER_COUNT);
        ControllerStateChangedEvent?.Invoke(playerIndex, controller);
    }

    public void UnassignController(int playerIndex)
    {
        Controllers[playerIndex] = null;
        ActiveControllerCount--;
        Assert.IsTrue(ActiveControllerCount >= 0);
        ControllerStateChangedEvent?.Invoke(playerIndex, null);
    }
}