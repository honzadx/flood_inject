using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace FloodInject.Runtime
{
    public abstract class AManagedStreamSO : AStreamSO
    { 
        public enum Phase
        {
            Map,
            Resolve
        }
        
        private Phase _currentPhase = Phase.Map;
        
        public Phase currentPhase => _currentPhase;
        
        public virtual Type[] RequiredContracts() => Array.Empty<Type>();
        
        public void ChangePhase(Phase newPhase)
        {
            if (_currentPhase != newPhase)
            {
                Debug.LogWarning($"{GetType().Name} is already set to {newPhase}");
                return;
            }
            
            switch (newPhase)
            {
                case Phase.Map:
                    UnmapAllInternal();
                    break;
                case Phase.Resolve:
                    bool missingRequiredContracts = false;
                    foreach (var type in RequiredContracts())
                    {
                        missingRequiredContracts |= !IsMapped(type);
                    }
                    Assert.IsTrue(missingRequiredContracts == false, $"Missing required contracts for {GetType().Name}");
                    break;
            }
            _currentPhase = newPhase;
        }
        
        public override void Map<T>(T value)
        {
            Assert.IsTrue(_currentPhase == Phase.Map);
            MapInternal(value);
        }

        public override void Map<T>(Func<T> factoryMethod)
        {
            Assert.IsTrue(_currentPhase == Phase.Map);
            MapInternal(factoryMethod);
        }

        public override void Remap<T>(T value)
        {
            Assert.IsTrue(_currentPhase == Phase.Map);
            RemapInternal(value);
        }

        public override void Remap<T>(Func<T> factoryMethod)
        {
            Assert.IsTrue(_currentPhase == Phase.Map);
            RemapInternal(factoryMethod);
        }

        public override void Unmap<T>()
        {
            Assert.IsTrue(_currentPhase == Phase.Map);
            UnmapInternal<T>();
        }
        
        public override void UnmapAll()
        {
            Assert.IsTrue(_currentPhase == Phase.Map);
            UnmapAllInternal();
        }

        public override T Resolve<T>()
        {
            Assert.IsTrue(_currentPhase == Phase.Resolve);
            return ResolveInternal<T>();
        }
    }
}