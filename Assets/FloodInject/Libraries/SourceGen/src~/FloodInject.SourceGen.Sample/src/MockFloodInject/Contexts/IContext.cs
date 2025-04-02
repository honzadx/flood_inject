using System;

namespace FloodInject.Runtime
{
    public interface IContext
    {
        public void Bind<T>(T value);
        public void Bind<T>(Func<T> factoryMethod);
    
        public void Rebind<T>(T value);
        public void Rebind<T>(Func<T> factoryMethod);
    
        public void Unbind<T>();
    
        public T Get<T>();
    
        public void Reset();
    }
}