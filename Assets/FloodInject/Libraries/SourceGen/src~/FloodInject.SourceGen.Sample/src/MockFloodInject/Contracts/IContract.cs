namespace FloodInject.Runtime
{
    internal interface IContract
    {
        public T Fulfill<T>();
    }
}