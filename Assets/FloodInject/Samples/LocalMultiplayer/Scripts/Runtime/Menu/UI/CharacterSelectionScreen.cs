using System;
using FloodInject.Runtime;
using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class CharacterSelectionScreen : MonoBehaviour
    {
        [SerializeField] private CharacterSelectionElement[] _characterSelectionElements;
        [SerializeField] private ButtonUI _menuButton;
        [SerializeField] private ButtonUI _playButton;
        [SerializeField] private MainMenuScreen _mainMenuScreen;

        protected void Start()
        {
            foreach (var element in _characterSelectionElements)
            {
                element.StateChangedEvent += OnStateChanged;
            }
            _menuButton.ButtonPressedEvent += OnMenuButtonPressed;
            _playButton.ButtonPressedEvent += OnPLayButtonPressed;
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

        private void OnPLayButtonPressed()
        {
            ContextProvider.GetContext().Get<GameScenesManager>().TransitionToScene("1_Game");
        }

        private void OnMenuButtonPressed()
        {
            _mainMenuScreen.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        
        public void OnSelectCharacter(int playerIndex, CharacterTemplateSO characterTemplate)
        {
            switch (playerIndex)
            {
                case 1:
                    ContextProvider.GetContext<Player1Context>().Rebind(characterTemplate);
                    break;
                case 2:
                    ContextProvider.GetContext<Player2Context>().Rebind(characterTemplate);
                    break;
                case 3:
                    ContextProvider.GetContext<Player3Context>().Rebind(characterTemplate);
                    break;
                case 4:
                    ContextProvider.GetContext<Player4Context>().Rebind(characterTemplate);
                    break;
                default: throw new IndexOutOfRangeException($"Received invalid player index {playerIndex}");
            }
        }
    }
}