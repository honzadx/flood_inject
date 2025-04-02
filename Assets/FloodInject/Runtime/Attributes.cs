using System;

namespace FloodInject.Runtime
{
    
    [AttributeUsage(AttributeTargets.Class)]
    public class FloodAttribute : Attribute
    {
        public bool IsOverride { get; }

        public FloodAttribute(bool isOverride = false)
        {
            IsOverride = isOverride;
        }
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class ResolveAttribute : Attribute 
    {
        /// <summary>Global context resolution</summary>
        public ResolveAttribute() { }
        
        /// <summary>Scriptable context resolution</summary>
        public ResolveAttribute(Type scriptableContext) { }
        
        /// <summary>Scene context resolution</summary>
        public ResolveAttribute(string sceneContext) { }
    }
}