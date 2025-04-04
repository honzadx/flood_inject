using FloodInject.Runtime;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class MainMenuGameMode : BaseGameMode
{
    [SerializeField] HeroSelection _heroSelection;

    protected void Start()
    {
        ContextProvider<GlobalContext>.Get().Rebind(_heroSelection);
        GameModeContext = ContextProvider<MainMenuContext>.Get();
        Initialize();
    }

    protected void OnDestroy()
    {
        GameModeContext.Reset();
        Deinitialize();
    }
}
