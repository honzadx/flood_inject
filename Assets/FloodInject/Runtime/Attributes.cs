using System;

namespace FloodInject.Runtime
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FloodAttribute : Attribute { }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class FloodResolveAttribute : Attribute 
    {
        public FloodResolveAttribute() { }
        public FloodResolveAttribute(Type stream) { }
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class FloodStreamRequirementAttribute : Attribute 
    { 
        public FloodStreamRequirementAttribute(Type contract) { }
    }
}