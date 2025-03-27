using System;
using FloodInject.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LocalMultiplayer.Runtime
{
    public class CharacterSelectionElement : MonoBehaviour
    {
        public event Action<int, State> StateChangedEvent;
        
        public enum State
        {
            Inactive,
            Registered,
            Ready
        }

        public record Result(bool IsPlaying, CharacterTemplateSO CharacterTemplate)
        {
            public bool IsPlaying { get; } = IsPlaying;
            public CharacterTemplateSO CharacterTemplate { get; } = CharacterTemplate;
        }
        
        [SerializeField] private Image _characterPortrait;
        [SerializeField] private TextMeshProUGUI _textElement;
        [SerializeField] private CharacterSelectionSO _characterSelection;
        [SerializeField] private Image _overlay;
        [SerializeField] private Image _colorTag;
        [SerializeField] private InputIconElement _inputIconElement;
        [SerializeField] private int _playerIndex = -1;
        [SerializeField] private Material _material;

        private State _currentState = State.Inactive;
        private int _currentCharacterIndex;
        private Material _materialInstance; 
        private PlayerInputRelay _playerInputRelay;
        
        public State CurrentState => _currentState;

        public Result GetResult()
        {
            return new Result(
                IsPlaying: _currentState == State.Ready,
                CharacterTemplate: _characterSelection.Characters[_currentCharacterIndex]);
        }

        protected void Start()
        {
            RefreshColor();
        }

        protected void OnEnable()
        {
            RandomizeCharacter();
            
            StartListeningToPlayerInput();
            RefreshElement();
            RefreshInputIcon();
        }

        protected void OnDisable()
        {
            StopListeningToPlayerInput();
            SetState(State.Inactive);
        }
        
        private void SetState(State newState)
        {
            _currentState = newState;
            StateChangedEvent?.Invoke(_playerIndex, _currentState);
            RefreshElement();
        }
        
        private void OnInputDeviceChanged(InputDevice inputDevice)
        {
            if(inputDevice == InputDevice.None)
            {
                SetState(State.Inactive);
            }
            RefreshInputIcon();
        }

        private void RandomizeCharacter()
        {
            _currentCharacterIndex = UnityEngine.Random.Range(0, _characterSelection.Characters.Length);
        }

        private void RefreshColor()
        {
            var playerColor = ContextProvider<GameContext>.Ctx
                .Get<GameInitSO>()
                .PlayerColors[_playerIndex];
            _materialInstance = new Material(_material);
            _materialInstance.SetColor(Statics.ToColorPropertyID, playerColor);
            _colorTag.color = playerColor;
            _characterPortrait.material = _materialInstance;
        }

        private void RefreshElement()
        {
            switch (_currentState)
            {
                case State.Inactive:
                    _overlay.gameObject.SetActive(true);
                    _textElement.text = "<NO_PLAYER>";
                    break;
                case State.Registered:
                    _overlay.gameObject.SetActive(false);
                    _textElement.text = $"<PLAYER_{_playerIndex}_REGISTERED>";
                    break;
                case State.Ready:
                    _overlay.gameObject.SetActive(false);
                    _textElement.text = $"<PLAYER_{_playerIndex}_READY>";
                    break;
            }
            RefreshPortrait();
        }

        private void RefreshInputIcon()
        {
            _inputIconElement.Init(_playerInputRelay.InputDevice);
            _colorTag.gameObject.SetActive(_playerInputRelay.InputDevice != InputDevice.None);
        }

        private void RefreshPortrait()
        {
            _characterPortrait.sprite = _characterSelection.Characters[_currentCharacterIndex].Portrait;
        }

#region INPUT
        private void StartListeningToPlayerInput()
        {
            _playerInputRelay = ContextProvider<GameContext>.Ctx
                .Get<GameInputManager>()
                .GetInputRelay(_playerIndex);
            _playerInputRelay.InputDeviceChangedEvent += OnInputDeviceChanged;
            _playerInputRelay.MoveEvent += OnMoveEvent;
            _playerInputRelay.Action1Event += OnAction1Event;
            _playerInputRelay.Action2Event += OnAction2Event;
        }

        private void StopListeningToPlayerInput()
        {
            if (_playerInputRelay == null) return;
            _playerInputRelay.InputDeviceChangedEvent -= OnInputDeviceChanged;
            _playerInputRelay.MoveEvent -= OnMoveEvent;
            _playerInputRelay.Action1Event -= OnAction1Event;
            _playerInputRelay.Action2Event -= OnAction2Event;
        }
        
        private void OnMoveEvent(Vector2 movement)
        {
            if (_currentState != State.Registered)
            {
                return;
            }
            
            var horizontal = movement.x;
            if (Mathf.Abs(horizontal) > 0.5f)
            {
                _currentCharacterIndex += (int)Mathf.Sign(horizontal);
                if (_currentCharacterIndex >= _characterSelection.Characters.Length)
                {
                    _currentCharacterIndex = 0;
                }
                else if (_currentCharacterIndex < 0)
                {
                    _currentCharacterIndex = _characterSelection.Characters.Length - 1;
                }
                RefreshPortrait();
            }
        }

        private void OnAction1Event(bool pressedDown)
        {
            if (!pressedDown) return;
            switch (_currentState)
            {
                case State.Inactive:
                    SetState(State.Registered);
                    break;
                case State.Registered:
                    SetState(State.Ready);
                    break;
            }
        }

        private void OnAction2Event(bool pressedDown)
        {
            if (!pressedDown) return;
            switch (_currentState)
            {
                case State.Registered:
                    SetState(State.Inactive);
                    break;
                case State.Ready:
                    SetState(State.Registered);
                    break;
            }
        }
#endregion INPUT
    }
}
