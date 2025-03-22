using FloodInject.Runtime;
using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    public class RegisterGameInit : MonoBehaviour
    {
        [SerializeField] private GameInitSO _gameInit;

        protected void Start()
        {
            ContextProvider<GameContext>.Ctx.Bind(_gameInit);
        }
    }
}