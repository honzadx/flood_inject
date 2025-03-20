using FloodInject.Runtime;
using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public partial class PlayerBehaviour : MonoBehaviour
    {
        [Inject(typeof(Player1Context))] public PlayerController _playerController;
    }
}