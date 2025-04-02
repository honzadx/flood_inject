using FloodInject.Runtime;
using UnityEngine;

namespace SourceGenerators.Sample.Tests;

public class HeroContext : ScriptableContext;
public class PlayerSettings;
public record HeroInit;
public interface IGameMode;

public static class HeroContextInstaller
{
    public static void Init(HeroInit initData)
    {
        ContextProvider.GetScriptableContext<HeroContext>().Bind(initData);
    }
}

public static class GameCoreSceneInstaller
{
    public static void Init(IGameMode gameMode)
    {
        ContextProvider.GetSceneContext("GameCoreScene").Bind(gameMode);
    }
}

[Flood]
public partial class Hero : MonoBehaviour {

    // Resolve from ScriptableContext
    [Resolve(typeof(HeroContext))] public HeroInit initData;
    
    // Resolve from SceneContext
    [Resolve("GameCoreScene")] public IGameMode gameMode;
    
    // Resolve from GlobalContext
    [Resolve] public PlayerSettings playerSettings;
}