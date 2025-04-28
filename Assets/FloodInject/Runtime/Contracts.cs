using System;

namespace FloodInject.Runtime
{
    internal abstract class AContract
    {
        public abstract T Resolve<T>();
    }
    
    internal sealed class InstanceContract<T> : AContract
    {
        private readonly T _instance;

        public InstanceContract(T instance)
        {
            _instance = instance;
        }

        public override T1 Resolve<T1>()
        {
            if (_instance is not T1 baseInstance)
            {
                throw new Exception($"Instance is of unexpected type {typeof(T)}");
            }
            return baseInstance;
        }
    }
    
    internal sealed class TransientContract<T> : AContract
    {
        private readonly Func<T> _factoryMethod;
        
        public TransientContract(Func<T> factoryMethod)
        {
            _factoryMethod = factoryMethod;
        }

        public override T1 Resolve<T1>()
        {
            if (_factoryMethod() is not T1 baseInstance)
            {
                throw new Exception($"Factory method returned unexpected type {typeof(T)}");
            }
            return baseInstance;
        }
    }
}