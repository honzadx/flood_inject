using FloodInject.Runtime;
using UnityEngine;

[Flood]
public partial class EngagementScreen : BaseScreen
{
    [SerializeField] BaseScreen _nextScreen;

    [Resolve(typeof(PlayerContext))] PlayerManager _playerManager;
    
    private void Start()
    {
        Construct();
        if (_playerManager.ActiveControllerCount <= 0)
        {
            _playerManager.ControllerStateChangedEvent += OnControllerStateChanged;
            return;
        }
        OpenNextScreen();
    }

    private void OnControllerStateChanged(int playerIndex, PlayerController playerController)
    {
        if (_playerManager.ActiveControllerCount <= 0)
        {
            return;
        }
        OpenNextScreen();
    }

    private void OpenNextScreen()
    {
        _playerManager.ControllerStateChangedEvent -= OnControllerStateChanged;
        gameObject.SetActive(false);
        _nextScreen.gameObject.SetActive(true);
    }
}
