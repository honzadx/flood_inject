using System.Runtime.CompilerServices;

namespace FloodInject.Runtime
{
    internal sealed class InstanceContract<TInstance> : IContract
    {
        private TInstance _instance;

        public InstanceContract(TInstance instance)
        {
            _instance = instance;
        }

        public TBaseInstance Fulfill<TBaseInstance>()
        {
            return Unsafe.As<TInstance, TBaseInstance>(ref _instance);
        }
    }
}