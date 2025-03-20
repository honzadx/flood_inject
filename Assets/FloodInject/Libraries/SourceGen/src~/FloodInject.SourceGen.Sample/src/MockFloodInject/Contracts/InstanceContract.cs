using System;

namespace FloodInject.Runtime
{
    internal sealed class InstanceContract<TInstance> : IContract
    {
        private readonly TInstance _instance;

        public InstanceContract(TInstance instance)
        {
            _instance = instance;
        }

        public TBaseInstance Fulfill<TBaseInstance>()
        {
            if (_instance is not TBaseInstance baseService)
            {
                throw new Exception($"Service returned unexpected type {typeof(TInstance)}");
            }
            return baseService;
        }
    }
}