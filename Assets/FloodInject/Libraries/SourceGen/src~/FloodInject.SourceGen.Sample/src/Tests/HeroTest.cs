using FloodInject.Runtime;
using UnityEngine;

namespace SourceGenerators.Sample.Tests;

public class HeroContext : BaseContext;
public class PlayerContext : BaseContext;

public class GameSettings { }
public record HeroInit { }
public class PlayerController { }

[Flood]
public partial class Hero : MonoBehaviour
{
    [Resolve] private GameSettings _gameSettings;
    [Resolve(typeof(HeroContext))] private HeroInit _heroInit;
    [Resolve(typeof(PlayerContext))] private PlayerController _playerController;
    
    public void Start()
    {
        Construct();
    }
}