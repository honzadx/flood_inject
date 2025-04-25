using FloodInject.Runtime;
using UnityEngine;

namespace SourceGenerators.Sample.Tests;

[FloodStreamRequirement(typeof(Hero))]
public partial class HeroStreamSO : AManagedStreamSO;

public class PlayerStreamSO : ADynamicStreamSO;

public class GameSettings { }
public record HeroInit { }
public class PlayerController { }

[Flood]
public partial class Unit : MonoBehaviour
{
    [FloodResolve] private GameSettings _gameSettings;
    
    protected void Start()
    {
        Construct();
    }
}

[Flood]
public partial class Hero : Unit
{
    [FloodResolve(typeof(HeroStreamSO))] private HeroInit _heroInit;
    [FloodResolve(typeof(PlayerStreamSO))] private PlayerController _playerController;
    
    protected new void Start()
    {
        Construct();
    }
}