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
        [SerializeField] private CharacterTemplateSO[] _characterTemplates;
        [SerializeField] private Image _overlay;
        [SerializeField] private Image _colorTag;
        [SerializeField] private InputIconElement _inputIconElement;
        [SerializeField] private int _playerIndex = -1;

        private State _currentState = State.Inactive;
        private int _currentCharacterIndex;
        
        public State CurrentState => _currentState;

        public Result GetResult()
        {
            return new Result(
                IsPlaying: _currentState == State.Ready,
                CharacterTemplate: _characterTemplates[_currentCharacterIndex]);
        }

        protected void OnEnable()
        {
            StartListeningToPlayerInput();
            RefreshElement();
            RefreshColorTag();
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
            _inputIconElement.Init(inputDevice);
        }

        private void RefreshColorTag()
        {
            _colorTag.color = ContextProvider<GameContext>.Ctx
                .Get<GameConfigSO>()
                .PlayerColors[_playerIndex];
        }

        private void RefreshElement()
        {
            switch (_currentState)
            {
                case State.Inactive:
                    _overlay.gameObject.SetActive(true);
                    _inputIconElement.gameObject.SetActive(false);
                    _colorTag.gameObject.SetActive(false);
                    _textElement.text = "<NO_PLAYER>";
                    break;
                case State.Registered:
                    _overlay.gameObject.SetActive(false);
                    _inputIconElement.gameObject.SetActive(true);
                    _colorTag.gameObject.SetActive(true);
                    _textElement.text = $"<PLAYER_{_playerIndex}_REGISTERED>";
                    break;
                case State.Ready:
                    _overlay.gameObject.SetActive(false);
                    _inputIconElement.gameObject.SetActive(true);
                    _colorTag.gameObject.SetActive(true);
                    _textElement.text = $"<PLAYER_{_playerIndex}_READY>";
                    break;
            }
            RefreshPortrait();
        }

        private void RefreshPortrait()
        {
            _characterPortrait.sprite = _characterTemplates[_currentCharacterIndex].Portrait;
        }

#region INPUT
        private void StartListeningToPlayerInput()
        {
            var context = ContextProvider<GameContext>.Ctx;
            var relay = context.Get<GameInputManager>().GetInputRelay(_playerIndex);
            relay.InputDeviceChangedEvent += OnInputDeviceChanged;
            relay.MoveEvent += OnMoveEvent;
            relay.Action1Event += OnAction1Event;
            relay.Action2Event += OnAction2Event;
        }

        private void StopListeningToPlayerInput()
        {
            var context = ContextProvider<GameContext>.Ctx;
            var relay = context.Get<GameInputManager>().GetInputRelay(_playerIndex);
            relay.InputDeviceChangedEvent -= OnInputDeviceChanged;
            relay.MoveEvent -= OnMoveEvent;
            relay.Action1Event -= OnAction1Event;
            relay.Action2Event -= OnAction2Event;
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
                if (_currentCharacterIndex >= _characterTemplates.Length)
                {
                    _currentCharacterIndex = 0;
                }
                else if (_currentCharacterIndex < 0)
                {
                    _currentCharacterIndex = _characterTemplates.Length - 1;
                }
                RefreshPortrait();
            }
        }

        private void OnAction1Event()
        {
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

        private void OnAction2Event()
        {
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
