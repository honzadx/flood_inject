using System;
using System.Runtime.CompilerServices;

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
            var instance = _factoryMethod();
            return Unsafe.As<TInstance, TBaseInstance>(ref instance);
        }
    }
}