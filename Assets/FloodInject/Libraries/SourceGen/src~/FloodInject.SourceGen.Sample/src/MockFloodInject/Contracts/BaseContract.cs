namespace FloodInject.Runtime
{
    internal abstract class BaseContract
    {
        public abstract ContractType ContractType { get; }
        public abstract T Fulfill<T>();
    }
}