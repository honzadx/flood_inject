using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace FloodInject.Runtime
{
    public abstract class BaseContext : ScriptableObject
    {
        public abstract System.Type ContextType { get; }

        private readonly Dictionary<Type, BaseContract> _contracts = new ();

        public void Register()
        {
            ContextProvider.Register(this);
        }
    
        public void Unregister()
        {
            ContextProvider.Register(this);
        }
        
        public void Bind<T>(T value)
        {
            var key = typeof(T);
            Assert.IsTrue(!_contracts.ContainsKey(key));
            _contracts.Add(key, new DirectContract<T>(value));
        }

        public void Bind<T>(Func<T> factoryMethod)
        {
            var key = typeof(T);
            Assert.IsTrue(!_contracts.ContainsKey(key));
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
            Assert.IsTrue(_contracts.ContainsKey(key));
            _contracts.Remove(key);
        }
        
        public T Get<T>()
        {
            var key = typeof(T);
            Assert.IsTrue(_contracts.ContainsKey(key));
            return _contracts[key].Fulfill<T>();
        }
        
        public void Reset()
        {
            _contracts.Clear();
        }
    }
}