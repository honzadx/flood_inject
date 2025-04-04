namespace FloodInject.Runtime
{
    internal abstract class BaseContract
    {
        public abstract T Resolve<T>();
    }
}