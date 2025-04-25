using UnityEngine;

namespace FloodInject.Runtime
{
    [DefaultExecutionOrder(-5)]
    public sealed class StreamLifetimeBehavior : MonoBehaviour
    {
        [SerializeField] private AStreamSO _stream;

        private void Awake()
        {
            StreamManager.instance.RegisterStream(_stream);
        }

        private void OnDestroy()
        {
            StreamManager.instance.UnregisterStream(_stream);
        }
    }   
}