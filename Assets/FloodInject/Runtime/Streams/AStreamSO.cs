using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FloodInject.Runtime
{
    public abstract class AStreamSO : ScriptableObject
    {
        private readonly Dictionary<Type, AContract> _contracts = new ();

        public abstract void Map<T>(T value);
        public abstract void Map<T>(Func<T> factoryMethod);
        public abstract void Remap<T>(T value);
        public abstract void Remap<T>(Func<T> factoryMethod);
        public abstract void Unmap<T>();
        public abstract void UnmapAll();
        public abstract T Resolve<T>();

        public bool IsMapped<T>()
        {
            return _contracts.ContainsKey(typeof(T));
        }

        public bool IsMapped(Type type)
        {
            return _contracts.ContainsKey(type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void MapInternal<T>(T value)
        {
            _contracts.Add(typeof(T), new InstanceContract<T>(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void MapInternal<T>(Func<T> factoryMethod)
        {
            _contracts.Add(typeof(T), new TransientContract<T>(factoryMethod));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RemapInternal<T>(T value)
        {
            _contracts[typeof(T)] = new InstanceContract<T>(value);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RemapInternal<T>(Func<T> factoryMethod)
        {
            _contracts[typeof(T)] = new TransientContract<T>(factoryMethod);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void UnmapInternal<T>()
        {
            _contracts.Remove(typeof(T));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void UnmapAllInternal()
        {
            _contracts.Clear();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected T ResolveInternal<T>()
        {
            return _contracts[typeof(T)].Resolve<T>();
        }
    }
}