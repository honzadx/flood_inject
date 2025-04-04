using UnityEngine;

namespace FloodInject.Runtime
{
    public static class ContextProvider<T> where T : BaseContext
    {
        private static T _context;
        
        static ContextProvider()
        {
            _context = ScriptableObject.CreateInstance<T>();
        }
        
        public static T Get() => _context;
    }
}
