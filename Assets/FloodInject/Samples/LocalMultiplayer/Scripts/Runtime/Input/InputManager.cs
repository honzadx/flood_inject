using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class InputManager : MonoBehaviour
    {
        protected void Start()
        {
            for (int playerIndex = Constants.PLAYER_INDEX_START; 
                 playerIndex < Constants.PLAYER_INDEX_START + Constants.MAX_PLAYER_COUNT; 
                 playerIndex++)
            {
                var context = PlayerContext.GetPlayerContextFromIndex(playerIndex);
                context.Bind(new PlayerInputRelay(playerIndex));
                Debug.Log($"Player input relay bound for index {playerIndex}");
            }
        }
    }
}