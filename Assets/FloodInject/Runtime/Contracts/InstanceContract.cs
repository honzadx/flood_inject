using Unity.Collections.LowLevel.Unsafe;

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
            return UnsafeUtility.As<TInstance, TBaseInstance>(ref _instance);
        }
    }
}