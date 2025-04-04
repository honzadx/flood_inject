using System;

namespace FloodInject.Runtime
{
    internal sealed class InstanceContract<T> : BaseContract
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
}