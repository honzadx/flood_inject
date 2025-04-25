using System;

namespace FloodInject.Runtime
{
    public abstract class ADynamicStreamSO : AStreamSO
    {
        public override void Map<T>(T value)
        {
            MapInternal(value);
        }

        public override void Map<T>(Func<T> factoryMethod)
        {
            MapInternal(factoryMethod);
        }

        public override void Remap<T>(T value)
        {
            RemapInternal(value);
        }

        public override void Remap<T>(Func<T> factoryMethod)
        {
            RemapInternal(factoryMethod);
        }

        public override void Unmap<T>()
        {
            UnmapInternal<T>();
        }

        public override void UnmapAll()
        {
            UnmapAllInternal();
        }

        public override T Resolve<T>()
        {
            return ResolveInternal<T>();
        }
    }
}