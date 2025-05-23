using FloodInject.Runtime;
using UnityEngine;

[DefaultExecutionOrder(-1)]
[Flood]
public partial class GameplayGameMode : BaseGameMode
{
    [SerializeField] HeroBehavior _heroBehaviorPrefab;
    
    [Resolve] HeroSelectionResult _heroSelectionResult;
    [Resolve(typeof(PlayerContext))] PlayerManager _playerManager;
    
    protected void Start()
    {
        Construct();
        foreach (var playerSelection in _heroSelectionResult.PlayerSelections)
        {
            var heroInstance = Instantiate(_heroBehaviorPrefab);
            var playerController = _playerManager.Controllers[playerSelection.PlayerIndex];
            var heroTemplate = playerSelection.SelectedHeroTemplate;
            heroInstance.Init(playerController, heroTemplate);
        }
        GameModeContext = ContextProvider<GameplayContext>.Get();
        Initialize();
    }

    protected void OnDestroy()
    {
        GameModeContext.Reset();
        Deinitialize();
    }
}
