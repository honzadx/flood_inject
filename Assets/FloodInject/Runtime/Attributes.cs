using System;

namespace FloodInject.Runtime
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GenerateContextAttribute : Attribute { }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class ContextListenerAttribute : Attribute
    {
        public bool IsOverride { get; }

        public ContextListenerAttribute(bool isOverride = false)
        {
            IsOverride = isOverride;
        }
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class InjectAttribute : Attribute 
    {
        public Type ContextType { get; }

        public InjectAttribute(Type contextType)
        {
            ContextType = contextType;
        }
    }
}