namespace FloodInject.Runtime
{
    internal abstract class AContract
    {
        public abstract T Resolve<T>();
    }
}