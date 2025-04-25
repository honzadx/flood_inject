using System;

namespace UnityEngine
{
    namespace Assertions
    {
        public static class Assert
        {
            public static bool IsTrue(bool condition)
            {
                return condition;
            }
            
            public static bool IsTrue(bool condition, string failureMessage)
            {
                if (!condition)
                {
                    Debug.LogWarning(failureMessage);
                }
                return condition;
            }
        }
    }
    
    public class ScriptableObject
    {
        public static T CreateInstance<T>() where T : ScriptableObject, new()
        {
            return new T();
        }
    }
    
    public class MonoBehaviour;

    public class DefaultExecutionOrderAttribute : Attribute
    {
        public DefaultExecutionOrderAttribute(int order) { }
    }

    public class SerializeFieldAttribute : Attribute { }
    
    public class RuntimeInitializeOnLoadMethodAttribute : Attribute
    {
        public RuntimeInitializeOnLoadMethodAttribute(RuntimeInitializeLoadType initType) { }
    }

    public enum RuntimeInitializeLoadType
    {
        SubsystemRegistration
    }

    public static class Debug
    {
        public static void Log(string message)
        {
            Console.WriteLine(message);
        }
        
        public static void LogWarning(string message)
        {
            Console.WriteLine(message);
        }
    }
}

