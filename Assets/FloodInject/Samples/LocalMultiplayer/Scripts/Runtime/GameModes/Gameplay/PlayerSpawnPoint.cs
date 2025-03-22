using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        [SerializeField] PlayerBehaviour _playerBehaviour;
        
        public void SpawnPlayer(int playerIndex, CharacterTemplateSO characterTemplate, PlayerInputRelay playerInputRelay)
        {
            var playerBehaviour = Instantiate(_playerBehaviour, transform);
            playerBehaviour.transform.parent = null;
            playerBehaviour.Init(playerIndex, characterTemplate, playerInputRelay);
        }
    }
}