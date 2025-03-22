using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FloodInject.Runtime
{
    public abstract class BaseContext
    {
        private readonly Dictionary<Type, IContract> _contracts = new ();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Bind<T>(T instance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Bind<T>(Func<T> factoryMethod);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Rebind<T>(T instance);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Rebind<T>(Func<T> factoryMethod);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Unbind<T>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract T Get<T>();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract void Reset();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void BindInternal<T>(T instance)
        {
            var key = typeof(T);
            _contracts.Add(key, new InstanceContract<T>(instance));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void BindInternal<T>(Func<T> factoryMethod)
        {
            var key = typeof(T);
            _contracts.Add(key, new TransientContract<T>(factoryMethod));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RebindInternal<T>(T instance)
        {
            var key = typeof(T);
            _contracts[key] = new InstanceContract<T>(instance);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RebindInternal<T>(Func<T> factoryMethod)
        {
            var key = typeof(T);
            _contracts[key] = new TransientContract<T>(factoryMethod);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void UnbindInternal<T>()
        {
            var key = typeof(T);
            _contracts.Remove(key);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected T GetInternal<T>()
        {
            var key = typeof(T);
            return _contracts[key].Fulfill<T>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void ResetInternal()
        {
            _contracts.Clear();
        }
    }
}