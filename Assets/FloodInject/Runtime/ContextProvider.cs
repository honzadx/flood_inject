using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace FloodInject.Runtime
{
    public static class ContextProvider
    {
        internal static readonly GlobalContext GlobalContext = new ();
        internal static readonly SceneContextManager SceneContextManager = new();
        internal static readonly Dictionary<Type, ScriptableContext> ScriptableContexts = new();
        
        public static void RegisterScriptableContext<T>(T context) where T : ScriptableContext
        {
            ScriptableContexts.Add(typeof(T), context);
        }

        public static void UnregisterScriptableContext<T>() where T : ScriptableContext
        {
            ScriptableContexts.Remove(typeof(T));
        }

        public static T GetScriptableContext<T>() where T : ScriptableContext
        {
            return (T)ScriptableContexts[typeof(T)];
        }
        
        public static SceneContext GetSceneContext(string sceneName)
        {
            return SceneContextManager.SceneContexts[sceneName];
        }

        public static GlobalContext GetGlobalContext()
        {
            return GlobalContext;
        }
    }
    
    internal sealed class SceneContextManager
    {
        internal readonly Dictionary<string, SceneContext> SceneContexts = new();

        internal SceneContextManager()
        {
            int sceneIndex = SceneManager.sceneCount - 1;
            for (; sceneIndex >= 0; sceneIndex--)
            {
                var scene = SceneManager.GetSceneAt(sceneIndex);
                var sceneContext = new SceneContext(scene.name);
                SceneContexts[sceneContext.SceneName] = sceneContext;
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            var sceneContext = new SceneContext(scene.name);
            SceneContexts[sceneContext.SceneName] = sceneContext;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            SceneContexts.Remove(scene.name);
        }
    }
}