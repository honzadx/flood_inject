using System;
using UnityEngine;

namespace FloodInject.Runtime
{
    [DefaultExecutionOrder(-2000)]
    public class ContextLifetime : MonoBehaviour
    {
        [SerializeField] private BaseContext[] _contexts;

        public void Awake()
        {
            foreach (var context in _contexts)
            {
                context.Register();
            }
        }

        private void OnDestroy()
        {
            foreach (var context in _contexts)
            {
                context.Unregister();
            }
        }
    }
}