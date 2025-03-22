using FloodInject.Runtime;

namespace LocalMultiplayer.Runtime
{
    /// <summary>
    /// 4 PLAYER LOCAL CO-OP GAME
    /// </summary>
    
    [GenerateContext(ContextType.Volatile)]
    public partial class GameContext { }
    
    [GenerateContext(ContextType.Protected)]
    public partial class PlayStartupContext { }
}