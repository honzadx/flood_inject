using FloodInject.Runtime;

namespace LocalMultiplayer.Runtime
{
    /// <summary>
    /// 4 PLAYER LOCAL CO-OP GAME
    /// </summary>
    
    [GenerateContext]
    public partial class Player1Context : BaseContext { }
    [GenerateContext]
    public partial class Player2Context : BaseContext { }
    [GenerateContext]
    public partial class Player3Context : BaseContext { }
    [GenerateContext]
    public partial class Player4Context : BaseContext { }
}