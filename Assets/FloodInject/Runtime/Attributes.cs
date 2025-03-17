using System;

namespace FloodInject.Runtime
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GenerateContextAttribute : Attribute { }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class ContextListenerAttribute : Attribute
    {
        public bool IsOverride { get; }
        public AutoInjectType AutoInjectType { get; }

        public ContextListenerAttribute(bool isOverride = false, AutoInjectType autoInjectType = AutoInjectType.None)
        {
            IsOverride = isOverride;
            AutoInjectType = autoInjectType;
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