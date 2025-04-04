using System.Linq;
using FloodInject.Runtime;
using UnityEngine;

[Flood]
public partial class HeroSelectionScreen : BaseScreen
{
    [SerializeField] HeroSelectionCard[] _selectionCards;
    [SerializeField] ButtonUIElement _startButton; 
    
    [Resolve] GameScenesManager _gameScenesManager;

    private HeroSelectionCard.State[] _heroSelectionStates = new HeroSelectionCard.State[Constants.MAX_PLAYER_COUNT];

    private void Start()
    {
        Construct();
        _startButton.ClickedEvent += OnStartButtonClicked;
        foreach (var selectionCard in _selectionCards)
        {
            selectionCard.Init();
            selectionCard.PlayerStateChanged += OnPlayerStateChanged;
            _heroSelectionStates[selectionCard.PlayerIndex] = selectionCard.HeroSelectionState;
        }
        RefreshVisual();
    }

    private void OnStartButtonClicked()
    {
        var playerSelections = _selectionCards
            .Select(selectionCard => selectionCard.GetPlayerSelection())
            .Where(playerSelection => playerSelection != null)
            .ToArray();
        var heroSelectionResult = new HeroSelectionResult(playerSelections);
        ContextProvider<GlobalContext>.Get().Rebind(heroSelectionResult);
        _gameScenesManager.TransitionToScene(Constants.GAMEPLAY_SCENE);
    }

    private void OnPlayerStateChanged(int playerIndex, HeroSelectionCard.State state)
    {
        _heroSelectionStates[playerIndex] = state;
        RefreshVisual();
    }

    private void RefreshVisual()
    {
        int noDevicePlayerCount = 0;
        int selectingPlayerCount = 0;
        int readyPlayerCount = 0;
        foreach (var heroSelectionState in _heroSelectionStates)
        {
            switch (heroSelectionState)
            {
                case HeroSelectionCard.State.NoDevice:
                    noDevicePlayerCount++;
                    break;
                case HeroSelectionCard.State.SelectingHero:
                    selectingPlayerCount++;
                    break;
                case HeroSelectionCard.State.Ready:
                    readyPlayerCount++;
                    break;
            }
        }
        Debug.Log($"[HeroSelectionScreen] {noDevicePlayerCount}/{selectingPlayerCount}/{readyPlayerCount}");
        _startButton.Interactable = readyPlayerCount >= 1 && selectingPlayerCount == 0;
    }
}