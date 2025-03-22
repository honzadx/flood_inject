using FloodInject.Runtime;
using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class GameInputManager : MonoBehaviour
    {
        private PlayerInputRelay[] _playerInputRelays;
        
        protected void Start()
        {
            ContextProvider<GameContext>.Ctx.Bind(this);

            _playerInputRelays = new PlayerInputRelay[Constants.MAX_PLAYER_COUNT];
            for(int playerIndex = 0; playerIndex < Constants.MAX_PLAYER_COUNT; ++playerIndex)
            {
                _playerInputRelays[playerIndex] = new PlayerInputRelay(playerIndex);
            }
        }

        public PlayerInputRelay GetInputRelay(int playerIndex)
        {
            return _playerInputRelays[playerIndex];
        }
    }
}