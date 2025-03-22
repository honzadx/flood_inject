using FloodInject.Runtime;
using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class RegisterGameConfigBehaviour : MonoBehaviour
    {
        [SerializeField] private GameConfigSO _gameConfig;

        protected void Start()
        {
            ContextProvider<GameContext>.Ctx.Bind(_gameConfig);
        }
    }
}