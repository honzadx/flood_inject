using System;

namespace FloodInject.Runtime
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GenerateContextAttribute : Attribute
    {
        public ContextType Definition { get; }

        public GenerateContextAttribute(ContextType definition)
        {
            Definition = definition;
        }
    }
    
    [AttributeUsage(AttributeTargets.Class)]
    public class ContextListenerAttribute : Attribute
    {
        public bool IsOverride { get; }
        public AutoInject AutoInject { get; }

        public ContextListenerAttribute(bool isOverride = false, AutoInject autoInject = AutoInject.None)
        {
            IsOverride = isOverride;
            AutoInject = autoInject;
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