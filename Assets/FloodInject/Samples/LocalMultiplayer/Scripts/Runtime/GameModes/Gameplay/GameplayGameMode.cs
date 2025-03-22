using FloodInject.Runtime;
using LocalMultiplayer.Runtime.UI;
using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class GameplayGameMode : BaseGameMode
    {
        [SerializeField] private PlayerSpawnPoint _playerSpawnPoint;
        [SerializeField] private Transform _playerFramesContainer;
        [SerializeField] private PlayerFrameDecorator _playerFramePrefab;
        
        protected void Start()
        {
            // Spawn players
            var gameInputManager = ContextProvider<GameContext>.Ctx.Get<GameInputManager>();
            var characterSelectionResult = ContextProvider<PlayStartupContext>.Ctx.Get<CharacterSelectionScreen.Result>();
            var playerIndex = 0;
            foreach (var characterSelection in characterSelectionResult.CharacterSelections)
            {
                if (characterSelection.IsPlaying)
                {
                    var player = _playerSpawnPoint.SpawnPlayer(
                        playerIndex, 
                        characterSelection.CharacterTemplate, 
                        gameInputManager.GetInputRelay(playerIndex));
                    var playerFrameInstance = Instantiate(_playerFramePrefab, _playerFramesContainer);
                    playerFrameInstance.Init(player);
                }
                playerIndex++;
            }
        }
    }
}