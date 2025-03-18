namespace FloodInject.Runtime
{
    [GenerateContext]
    public sealed partial class GlobalContext : BaseContext
    {
        private void Awake()
        {
            Register();
        }

        private void OnDestroy()
        {
            Unregister();
        }
    }
}