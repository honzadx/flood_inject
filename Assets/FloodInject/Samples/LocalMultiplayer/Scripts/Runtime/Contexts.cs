using System;
using FloodInject.Runtime;

namespace LocalMultiplayer.Runtime
{
    /// <summary>
    /// 4 PLAYER LOCAL CO-OP GAME
    /// </summary>
    
    [GenerateContext(ContextType.Volatile)]
    public partial class GameContext { }
    [GenerateContext(ContextType.Volatile)]
    public partial class Player1Context { }
    [GenerateContext(ContextType.Volatile)]
    public partial class Player2Context { }
    [GenerateContext(ContextType.Volatile)]
    public partial class Player3Context { }
    [GenerateContext(ContextType.Volatile)]
    public partial class Player4Context { }
    
    public static class PlayerContext 
    {
        public static BaseContext GetPlayerContextFromIndex(int playerIndex)
        {
            return playerIndex switch
            {
                1 => ContextProvider<Player1Context>.GetContext(),
                2 => ContextProvider<Player2Context>.GetContext(),
                3 => ContextProvider<Player3Context>.GetContext(),
                4 => ContextProvider<Player4Context>.GetContext(),
                _ => throw new IndexOutOfRangeException($"Player index {playerIndex} is not supported")
            };
        }
    }
}