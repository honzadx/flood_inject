using System;
using Unity.Collections.LowLevel.Unsafe;

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
            return UnsafeUtility.As<TInstance, TBaseInstance>(ref instance);
        }
    }
}