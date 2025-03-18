using System;

namespace FloodInject.Runtime
{
    internal sealed class TransientContract<TService> : BaseContract
    {
        private Func<TService> _factoryMethod;
        
        public TransientContract(Func<TService> factoryMethod)
        {
            _factoryMethod = factoryMethod;
        }

        public override ContractType ContractType => ContractType.Transient;

        public override TBaseService Fulfill<TBaseService>()
        {
            if (_factoryMethod() is not TBaseService result)
            {
                throw new Exception($"Factory method returned unexpected type {typeof(TService)}");
            }
            return result;
        }
    }
}