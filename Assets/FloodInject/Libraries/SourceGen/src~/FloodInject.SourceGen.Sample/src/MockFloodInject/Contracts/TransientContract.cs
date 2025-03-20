using System;

namespace FloodInject.Runtime
{
    internal sealed class TransientContract<TInstance> : IContract
    {
        private readonly Func<TInstance> _factoryMethod;
        
        public TransientContract(Func<TInstance> factoryMethod)
        {
            _factoryMethod = factoryMethod;
        }

        public TBaseInstance Fulfill<TBaseInstance>()
        {
            if (_factoryMethod() is not TBaseInstance result)
            {
                throw new Exception($"Factory method returned unexpected type {typeof(TInstance)}");
            }
            return result;
        }
    }
}