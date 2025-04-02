using System;

namespace FloodInject.Runtime
{
    internal sealed class DirectContract<TService> : BaseContract
    {
        private TService _service;

        public DirectContract(TService service)
        {
            _service = service;
        }

        public override TBaseService Fulfill<TBaseService>()
        {
            if (_service is not TBaseService baseService)
            {
                throw new Exception($"Service returned unexpected type {typeof(TService)}");
            }
            return baseService;
        }
    }
}