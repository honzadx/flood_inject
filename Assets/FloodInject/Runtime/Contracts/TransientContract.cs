using System;
using UnityEngine.Assertions;

namespace FloodInject.Runtime
{
    internal sealed class TransientContract<T> : BaseContract
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