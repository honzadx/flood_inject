using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace FloodInject.Runtime
{
    public abstract class BaseContext : ScriptableObject
    {
        public abstract System.Type ContextType { get; }

        private readonly Dictionary<Type, object> _bindings = new ();

        public void Bind<T2>(T2 value)
        {
            var key = typeof(T2);
            Assert.IsTrue(!_bindings.ContainsKey(key));
            _bindings.Add(key, value);
        }
        
        public void Bind<T2>(Type key, T2 value)
        {
            Assert.IsTrue(!_bindings.ContainsKey(key));
            _bindings.Add(key, value);
        }

        public void Rebind<T2>(T2 value)
        {
            var key = typeof(T2);
            _bindings[key] = value;
        }
        
        public void Rebind<T2>(Type key, T2 value)
        {
            _bindings[key] = value;
        }

        public void Unbind<T2>()
        {
            var key = typeof(T2);
            Assert.IsTrue(_bindings.ContainsKey(key));
            _bindings.Remove(key);
        }
        
        public void Unbind(Type key)
        {
            Assert.IsTrue(_bindings.ContainsKey(key));
            _bindings.Remove(key);
        }

        public T2 Get<T2>()
        {
            var key = typeof(T2);
            Assert.IsTrue(_bindings.ContainsKey(key));
            return (T2)_bindings[key];
        }
        
        public object Get<T2>(Type key)
        {
            Assert.IsTrue(_bindings.ContainsKey(key));
            return (T2)_bindings[key];
        }
        
        public void Reset()
        {
            _bindings.Clear();
        }
    }
}