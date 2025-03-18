using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LocalMultiplayer.Runtime
{
    public record CharacterSelectionState(bool IsPlaying, CharacterTemplateSO CharacterTemplate)
    {
        public bool IsPlaying { get; } = IsPlaying;
        public CharacterTemplateSO CharacterTemplate { get; } = CharacterTemplate;
    }
    
    public class CharacterSelectionElement : MonoBehaviour
    {
        public event Action<int, State> StateChangedEvent;
        
        public enum State
        {
            Inactive,
            Registered,
            Ready
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

        public void AcceptSelection()
        {
            var characterSelectionState = new CharacterSelectionState(
                IsPlaying: _currentState == State.Ready,
                CharacterTemplate: _characterTemplates[_currentCharacterIndex]);
            var playerContext = PlayerContext.GetPlayerContextFromIndex(_playerIndex);
            playerContext.Bind(characterSelectionState);
        }

        public void OnEnable()
        {
            StartListeningToPlayerInput();
            RefreshElement();
        }

        public void OnDisable()
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
            var relay = PlayerContext.GetPlayerContextFromIndex(_playerIndex).Get<PlayerInputRelay>();
            relay.InputDeviceChangedEvent += OnInputDeviceChanged;
            relay.MoveEvent += OnMove;
            relay.Action1Event += OnAction1;
            relay.Action2Event += OnAction2;
        }

        private void StopListeningToPlayerInput()
        {
            var relay = PlayerContext.GetPlayerContextFromIndex(_playerIndex)?.Get<PlayerInputRelay>();
            if (relay != null)
            {
                relay.InputDeviceChangedEvent -= OnInputDeviceChanged;
                relay.MoveEvent -= OnMove;
                relay.Action1Event -= OnAction1;
                relay.Action2Event -= OnAction2;
            }
        }
        
        private void OnMove(Vector2 movement)
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

        private void OnAction1()
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

        private void OnAction2()
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
