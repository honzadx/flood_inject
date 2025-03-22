using FloodInject.Runtime;
using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class CharacterSelectionScreen : MonoBehaviour
    {
        public record Result(CharacterSelectionElement.Result[] CharacterSelections)
        {
            public CharacterSelectionElement.Result[] CharacterSelections { get; } = CharacterSelections;
        }
        
        [SerializeField] private CharacterSelectionElement[] _characterSelectionElements;
        [SerializeField] private ButtonUI _menuButton;
        [SerializeField] private ButtonUI _playButton;
        [SerializeField] private MainMenuScreen _mainMenuScreen;

        protected void Start()
        {
            ContextProvider<PlayStartupContext>.Ctx.Reset();
            foreach (var element in _characterSelectionElements)
            {
                element.StateChangedEvent += OnStateChanged;
            }
            _menuButton.ButtonPressedEvent += OnMenuButtonPressed;
            _playButton.ButtonPressedEvent += OnPlayButtonPressed;
            RefreshElement();
        }

        private void OnStateChanged(int playerIndex, CharacterSelectionElement.State newState)
        {
            RefreshElement();
        }

        private void RefreshElement()
        {
            int readyCount = 0;
            int registeredCount = 0;
            int inactiveCount = 0;
            foreach (var element in _characterSelectionElements)
            {
                switch (element.CurrentState)
                {
                    case CharacterSelectionElement.State.Ready:
                        readyCount++;
                        break;
                    case CharacterSelectionElement.State.Registered:
                        registeredCount++;
                        break;
                    case CharacterSelectionElement.State.Inactive:
                        inactiveCount++;
                        break;
                }
            }
            _playButton.SetInteractable(readyCount > 0 && registeredCount == 0);
            Debug.Log($"CharacterSelectionScreen selection state changed ({inactiveCount}|{registeredCount}|{readyCount})");
        }

        private void OnPlayButtonPressed()
        {
            CharacterSelectionElement.Result[] characterSelections = new CharacterSelectionElement.Result[_characterSelectionElements.Length];
            for (int playerIndex = 0; playerIndex < _characterSelectionElements.Length; ++playerIndex)
            {
                characterSelections[playerIndex] = _characterSelectionElements[playerIndex].GetResult();
            }
            var result = new Result(characterSelections);
            var context = ContextProvider<PlayStartupContext>.Ctx;
            context.Rebind(result);
            context.Lock();
            
            ContextProvider<GameContext>.Ctx
                .Get<GameScenesManager>()
                .TransitionToScene("1_Gameplay");
        }

        private void OnMenuButtonPressed()
        {
            _mainMenuScreen.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}