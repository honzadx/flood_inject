using System;
using System.Collections.Generic;

namespace FloodInject.Runtime
{
    public static class ContextProvider
    {
        private static readonly Dictionary<Type, BaseContext> _contexts = new();

        public static void Register<T>(T context) where T : BaseContext
        {
            _contexts.Add(context.ContextType, context);
        }

        public static void Unregister<T>(T context) where T : BaseContext
        {
            _contexts.Remove(context.ContextType);
        }

        public static T GetContext<T>() where T : BaseContext
        {
            return _contexts.TryGetValue(typeof(T), out BaseContext baseContext) ? baseContext as T : null;
        }

        public static GlobalContext GetContext()
        {
            return (GlobalContext)_contexts[typeof(GlobalContext)];
        }
    }
}
