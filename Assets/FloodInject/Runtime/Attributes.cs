using System;

namespace FloodInject.Runtime
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FloodAttribute : Attribute
    {
        public FloodAttribute(bool isOverride = false) { }
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class ResolveAttribute : Attribute 
    {
        public ResolveAttribute() { }
        public ResolveAttribute(Type contextType) { }
    }
}