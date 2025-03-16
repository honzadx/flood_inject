using System;

namespace FloodInject.Runtime;

[AttributeUsage(AttributeTargets.Field)]
public class InjectAttribute : Attribute 
{
    public Type ContextType { get; }

    public InjectAttribute(Type contextType)
    {
        ContextType = contextType;
    }
}