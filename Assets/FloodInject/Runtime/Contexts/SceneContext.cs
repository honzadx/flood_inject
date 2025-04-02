using System;
using System.Collections.Generic;

namespace FloodInject.Runtime
{
    public sealed class SceneContext : IContext
    {
        private readonly Dictionary<Type, BaseContract> _contracts = new ();
        
        public string SceneName { get; }
        
        public SceneContext(string sceneName)
        {
            SceneName = sceneName;
        }
        
        public void Bind<T>(T value)
        {
            var key = typeof(T);
            _contracts.Add(key, new DirectContract<T>(value));
        }
    
        public void Bind<T>(Func<T> factoryMethod)
        {
            var key = typeof(T);
            _contracts.Add(key, new TransientContract<T>(factoryMethod));
        }
    
        public void Rebind<T>(T value)
        {
            var key = typeof(T);
            _contracts[key] = new DirectContract<T>(value);
        }
        
        public void Rebind<T>(Func<T> factoryMethod)
        {
            var key = typeof(T);
            _contracts[key] = new TransientContract<T>(factoryMethod);
        }
        
        public void Unbind<T>()
        {
            var key = typeof(T);
            _contracts.Remove(key);
        }
        
        public T Get<T>()
        {
            var key = typeof(T);
            return _contracts[key].Fulfill<T>();
        }
        
        public void Reset()
        {
            _contracts.Clear();
        }
    }
}