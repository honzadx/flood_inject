using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FloodInject.Runtime
{
    public abstract class BaseContext
    {
        private readonly Dictionary<Type, IContract> _typeContracts = new ();
        private readonly Dictionary<string, IContract> _keyedContracts = new ();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Bind<T>(T instance);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Bind<T>(Func<T> factoryMethod);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Bind<T>(string key, T instance);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Bind<T>(string key, Func<T> factoryMethod);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Rebind<T>(T instance);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Rebind<T>(Func<T> factoryMethod);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Rebind<T>(string key, T instance);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Rebind<T>(string key, Func<T> factoryMethod);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Unbind<T>();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Unbind(string key);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract T Get<T>();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract T Get<T>(string key);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Reset();
        
#region Internal methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void BindInternal<T>(T instance)
        {
            _typeContracts.Add(typeof(T), new InstanceContract<T>(instance));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void BindInternal<T>(Func<T> factoryMethod)
        {
            _typeContracts.Add(typeof(T), new TransientContract<T>(factoryMethod));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void BindInternal<T>(string key, T instance)
        {
            _keyedContracts.Add(key, new InstanceContract<T>(instance));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void BindInternal<T>(string key, Func<T> factoryMethod)
        {
            _keyedContracts.Add(key, new TransientContract<T>(factoryMethod));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RebindInternal<T>(T instance)
        {
            _typeContracts[typeof(T)] = new InstanceContract<T>(instance);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RebindInternal<T>(Func<T> factoryMethod)
        {
            _typeContracts[typeof(T)] = new TransientContract<T>(factoryMethod);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RebindInternal<T>(string key, T instance)
        {
            _keyedContracts[key] = new InstanceContract<T>(instance);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RebindInternal<T>(string key, Func<T> factoryMethod)
        {
            _keyedContracts[key] = new TransientContract<T>(factoryMethod);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void UnbindInternal<T>()
        {
            var key = typeof(T);
            _typeContracts.Remove(key);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void UnbindInternal(string key)
        {
            _keyedContracts.Remove(key);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected T GetInternal<T>()
        {
            var key = typeof(T);
            return _typeContracts[key].Fulfill<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected T GetInternal<T>(string key)
        {
            return _keyedContracts[key].Fulfill<T>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void ResetInternal()
        {
            _typeContracts.Clear();
            _keyedContracts.Clear();
        }
#endregion
    }
}