using System.Collections.Generic;
using FloodInject.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LocalMultiplayer.Runtime
{
    public class GameScenesManager : MonoBehaviour
    {
        public void Start()
        {
            ContextProvider.GetContext<GlobalContext>().Bind(this);
            if (SceneManager.sceneCount == 1)
            {
                SceneManager.LoadSceneAsync(1);
            }
        }
        
        public void TransitionToScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        public static IEnumerable<Scene> GetAllLoadedScenes()
        {
            for(int i = 0; i < SceneManager.sceneCount; i++)
            {
                yield return SceneManager.GetSceneAt(i);
            }
        }
    }
}
