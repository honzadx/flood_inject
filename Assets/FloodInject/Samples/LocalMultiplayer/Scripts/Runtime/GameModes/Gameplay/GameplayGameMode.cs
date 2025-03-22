using FloodInject.Runtime;
using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class GameplayGameMode : BaseGameMode
    {
        [SerializeField] private PlayerSpawnPoint _playerSpawnPoint;
        
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
                    _playerSpawnPoint.SpawnPlayer(
                        playerIndex, 
                        characterSelection.CharacterTemplate, 
                        gameInputManager.GetInputRelay(playerIndex));
                }
                playerIndex++;
            }
        }
    }
}