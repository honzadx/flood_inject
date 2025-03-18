using System;
using FloodInject.Runtime;
using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    /// <summary>
    /// 4 PLAYER LOCAL CO-OP GAME
    /// </summary>
    
    [GenerateContext, CreateAssetMenu(menuName = "LocalMultiplayer/Context/Player1Context")]
    public partial class Player1Context : BaseContext { }
    [GenerateContext, CreateAssetMenu(menuName = "LocalMultiplayer/Context/Player2Context")]
    public partial class Player2Context : BaseContext { }
    [GenerateContext, CreateAssetMenu(menuName = "LocalMultiplayer/Context/Player3Context")]
    public partial class Player3Context : BaseContext { }
    [GenerateContext, CreateAssetMenu(menuName = "LocalMultiplayer/Context/Player4Context")]
    public partial class Player4Context : BaseContext { }
    
    public static class PlayerContext 
    {
        public static BaseContext GetPlayerContextFromIndex(int playerIndex)
        {
            return playerIndex switch
            {
                1 => ContextProvider.GetContext<Player1Context>(),
                2 => ContextProvider.GetContext<Player2Context>(),
                3 => ContextProvider.GetContext<Player3Context>(),
                4 => ContextProvider.GetContext<Player4Context>(),
                _ => throw new IndexOutOfRangeException($"Player index {playerIndex} is not supported")
            };
        }
    }
}