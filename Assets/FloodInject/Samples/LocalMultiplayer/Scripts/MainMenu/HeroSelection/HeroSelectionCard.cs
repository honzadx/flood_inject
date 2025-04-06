using System;
using FloodInject.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

[Flood]
public partial class HeroSelectionCard : MonoBehaviour
{
    public event Action<int, State> PlayerStateChanged;
    
    public enum State
    {
        NoDevice = 0, 
        SelectingHero = 1,
        Ready = 2,
    }
    
    [SerializeField] int _playerIndex;
    [SerializeField] Image _colorBackgroundImage;
    [SerializeField] Image _portraitImage;
    [SerializeField] TextMeshProUGUI _stateText;
    [SerializeField] ColorTemplate _inactiveColor;
    [SerializeField] ColorTemplate _activeColor;
    
    [Resolve(typeof(PlayerContext))] PlayerManager _playerManager;
    [Resolve] HeroSelection _heroSelection;

    private PlayerController _playerController;
    private State _state = State.NoDevice;
    private int _heroIndex;
    
    public int PlayerIndex => _playerIndex;
    public State HeroSelectionState => _state;
    
    public void Init()
    {
        Construct();

        _heroIndex = 0;
        Assert.IsTrue(_playerIndex < _playerManager.Controllers.Length);
        UnbindPlayerController(_playerController);
        BindPlayerController(_playerManager.Controllers[_playerIndex]);
        
        SetState(_playerController != null ? State.SelectingHero : State.NoDevice);
        RefreshVisual();
        
        _playerManager.ControllerStateChangedEvent -= OnControllerStateChanged;
        _playerManager.ControllerStateChangedEvent += OnControllerStateChanged;
    }

    private void OnDestroy()
    {
        UnbindPlayerController(_playerController);
        _playerManager.ControllerStateChangedEvent -= OnControllerStateChanged;
    }

    private void SetState(State state)
    {
        State prevState = _state;
        _state = state;
        PlayerStateChanged?.Invoke(_playerIndex, _state);
        if (prevState == State.NoDevice && _state == State.SelectingHero)
        {
            _heroIndex = 0;
        }
    }

    private void RefreshVisual()
    {
        var hero = _heroSelection.Heroes[_heroIndex];
        _portraitImage.sprite = hero.Portrait;
        _portraitImage.color = _state == State.NoDevice ? _inactiveColor.Color : _activeColor.Color;
        switch (_state)
        {
            case State.NoDevice:
                _stateText.text = "no device";
                _portraitImage.rectTransform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                _portraitImage.rectTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, 5.0f);
                break;
            case State.SelectingHero:
                _stateText.text = "hero selection";
                _portraitImage.rectTransform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                _portraitImage.rectTransform.localRotation = Quaternion.Euler(0.0f, 0.0f, 5.0f);
                break;
            case State.Ready:
                _stateText.text = "ready";
                _portraitImage.rectTransform.localScale = Vector3.one;
                _portraitImage.rectTransform.localRotation = Quaternion.identity;
                break;
        }
    } 
    
    private void BindPlayerController(PlayerController playerController)
    {
        _playerController = playerController;
        if (playerController == null)
        {
            return;
        }
        playerController.MoveEvent += OnMove;
        playerController.Action1Event += OnAction1;
        playerController.Action2Event += OnAction2;
    }
    
    private void UnbindPlayerController(PlayerController playerController)
    {
        _playerController = null;
        if (playerController == null)
        {
            return;
        }
        playerController.MoveEvent -= OnMove;
        playerController.Action1Event -= OnAction1;
        playerController.Action2Event -= OnAction2;
    }

    private void OnControllerStateChanged(int playerIndex, PlayerController playerController)
    {
        if (playerIndex != _playerIndex)
        {
            return;
        }
        var prevPlayerController = _playerController;
        UnbindPlayerController(prevPlayerController);
        BindPlayerController(playerController);
    }

    public HeroSelectionResult.PlayerSelection GetPlayerSelection()
    {
        return _state switch
        {
            State.Ready => new HeroSelectionResult.PlayerSelection(_playerIndex, _heroSelection.Heroes[_heroIndex]),
            _ => null
        };
    }

#region Player Input 
    private void OnMove(Vector2 moveDirection)
    {
        if (_state != State.SelectingHero || Mathf.Approximately(moveDirection.x, 0.0f))
        {
            return;
        }
        _heroIndex += (int)Mathf.Sign(moveDirection.x);
        _heroIndex = Mathf.Clamp(_heroIndex, 0, _heroSelection.Heroes.Length - 1);
        RefreshVisual();
    }
    
    private void OnAction1()
    {
        if (_state >= State.Ready)
        {
            return;
        }
        SetState(++_state);
        RefreshVisual();
    }

    private void OnAction2()
    {
        if (_state <= State.SelectingHero)
        {
            return;
        }
        SetState(--_state);
        RefreshVisual();
    }
#endregion
}
