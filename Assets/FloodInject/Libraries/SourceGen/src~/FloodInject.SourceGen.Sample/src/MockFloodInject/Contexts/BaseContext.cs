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
            var key = typeof(T);
            _contracts.Add(key, new InstanceContract<T>(value));
        }

        public void Bind<T>(Func<T> factoryMethod)
        {
            var key = typeof(T);
            _contracts.Add(key, new TransientContract<T>(factoryMethod));
        }

        public void Rebind<T>(T value)
        {
            var key = typeof(T);
            _contracts[key] = new InstanceContract<T>(value);
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
        
        public void Reset()
        {
            _contracts.Clear();
        }
        
        public T Resolve<T>()
        {
            var key = typeof(T);
            return _contracts[key].Fulfill<T>();
        }
    }
}