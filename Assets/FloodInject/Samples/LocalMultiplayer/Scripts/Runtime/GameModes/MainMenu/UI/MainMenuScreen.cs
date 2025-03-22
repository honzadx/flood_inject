using System;
using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class MainMenuScreen : MonoBehaviour
    {
        [SerializeField] private ButtonUI _startButton;
        [SerializeField] private ButtonUI _quitButton;
        [SerializeField] CharacterSelectionScreen _characterSelectionScreen;

        protected void Start()
        {
            _startButton.ButtonPressedEvent += OnStartButtonPressed;
            _quitButton.ButtonPressedEvent += OnQuitButtonPressed;
        }

        private void OnStartButtonPressed()
        {
            _characterSelectionScreen.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        private void OnQuitButtonPressed()
        {
            Application.Quit();
        }
    }
}
