using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        [SerializeField] PlayerBehaviour _player;
        
        public PlayerBehaviour SpawnPlayer(int playerIndex, CharacterTemplateSO characterTemplate, PlayerInputRelay playerInputRelay)
        {
            var player = Instantiate(_player, transform);
            player.transform.parent = null;
            player.Init(playerIndex, characterTemplate, playerInputRelay);
            return player;
        }
    }
}