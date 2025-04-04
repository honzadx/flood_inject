using System;
using System.Collections.Generic;
using UnityEngine;

namespace FloodInject.Runtime
{
    public abstract class BaseContext : ScriptableObject
    {
        private readonly Dictionary<Type, BaseContract> _contracts = new ();

        public void Bind<T>(T value)
        {
            _contracts.Add(typeof(T), new InstanceContract<T>(value));
        }

        public void Bind<T>(Func<T> factoryMethod)
        {
            _contracts.Add(typeof(T), new TransientContract<T>(factoryMethod));
        }

        public void Rebind<T>(T value)
        {
            _contracts[typeof(T)] = new InstanceContract<T>(value);
        }
        
        public void Rebind<T>(Func<T> factoryMethod)
        {
            _contracts[typeof(T)] = new TransientContract<T>(factoryMethod);
        }
        
        public void Unbind<T>()
        {
            _contracts.Remove(typeof(T));
        }
        
        public void Reset()
        {
            _contracts.Clear();
        }
        
        public bool Exists<T>()
        {
            return _contracts.ContainsKey(typeof(T));
        }
        
        public T Resolve<T>()
        {
            return _contracts[typeof(T)].Resolve<T>();
        }
    }
}