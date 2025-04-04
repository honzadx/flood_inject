using System.Collections;
using FloodInject.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneTransitionType
{
    DontReload,
    Reload
}

public class GameScenesManager : MonoBehaviour
{
    private bool _inTransition;
    
    public void Start()
    {
        ContextProvider<GlobalContext>.Get().Bind(this);
        if (SceneManager.sceneCount < 2)
        {
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }
    }

    public void TransitionToScene(string sceneName, SceneTransitionType transitionType = SceneTransitionType.DontReload)
    {
        if (_inTransition)
        {
            Debug.LogWarning("TransitionToScene called more than once");
            return;
        }
        var currentScene = SceneManager.GetSceneAt(1);
        if (transitionType == SceneTransitionType.DontReload &&  currentScene.name == sceneName)
        {
            return;
        }
        StartCoroutine(TransitionCoroutine(currentScene.name, sceneName));
    }

    private IEnumerator TransitionCoroutine(string currentSceneName, string nextSceneName)
    {
        _inTransition = true;
        var asyncOperation = SceneManager.UnloadSceneAsync(currentSceneName);
        if (asyncOperation != null)
        {
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
        }
        
        asyncOperation = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        if (asyncOperation != null)
        {
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
        }
        _inTransition = false;
    }
}