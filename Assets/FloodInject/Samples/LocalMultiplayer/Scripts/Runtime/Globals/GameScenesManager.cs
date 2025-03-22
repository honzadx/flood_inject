using FloodInject.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LocalMultiplayer.Runtime
{
    public class GameScenesManager : MonoBehaviour
    {
        public void Start()
        {
            ContextProvider<GameContext>.Ctx.Bind(this);
            if (SceneManager.sceneCount == 1)
            {
                SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            }
        }
        
        public void TransitionToScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
}
