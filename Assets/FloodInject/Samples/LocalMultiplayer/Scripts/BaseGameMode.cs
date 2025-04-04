using FloodInject.Runtime;
using UnityEngine;
using UnityEngine.Assertions;

[DefaultExecutionOrder(-1)]
public abstract class BaseGameMode : MonoBehaviour
{
    public static BaseContext GameModeContext;
    public static BaseGameMode Instance { get; protected set; }

    protected void Initialize()
    {
        Assert.IsNull(Instance);
        Instance = this;
    }

    protected void Deinitialize()
    {
        Assert.IsTrue(Instance == this);
        Instance = null;
    }
}
