using System;
using System.Collections.Generic;
using UnityEngine;

namespace FloodInject.Runtime
{
    public class StreamManager
    {
        private static StreamManager? _instance;
        public static StreamManager instance => _instance ??= new StreamManager();

        private readonly Dictionary<Type, AStreamSO> _streams = new ();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            _instance = null;
        }

        private StreamManager()
        {
            _streams.Add(typeof(SharedStreamSO), ScriptableObject.CreateInstance<SharedStreamSO>());
        }

        public T GetStream<T>() where T : AStreamSO
        {
            return (T)_streams[typeof(T)];
        }

        public void RegisterStream<T>(T stream) where T : AStreamSO
        {
            _streams.Add(typeof(T), stream);
        }

        public void UnregisterStream<T>(T stream) where T : AStreamSO
        {
            _streams.Remove(stream.GetType());
        }
    }
}